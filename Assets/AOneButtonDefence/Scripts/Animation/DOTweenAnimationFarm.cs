using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimationFarm : MonoBehaviour, IAnimatable
{
    [SerializeField] private GameObject human;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> doneHarvestPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> growingPlants = new List<GameObject>();
    [SerializeField] private float walkDuration;
    [SerializeField] private float harvestTime;
    [SerializeField] private float growTime;
    [SerializeField] private Vector3 endPlantScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private int harvestCount;

    private Coroutine currentAnimation;
    private GameObject worker;

    private void Start()
    {
        worker = Instantiate(human, spawnPoint.transform.position, Quaternion.identity);
    }

    public void StartAnimation() => currentAnimation = StartCoroutine(WorkerRoutine());

    public void InteruptAnimation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            StartCoroutine(StopWorking());
        }
    }

    private IEnumerator WorkerRoutine()
    {
        while (true)
        {
            if (doneHarvestPoints.Count < harvestCount)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            for (int i = 0; i < harvestCount; i++)
            {
                int randpoint = Random.Range(0, doneHarvestPoints.Count);
                var point = doneHarvestPoints[randpoint];
                worker.transform.DOMove(point.transform.position, walkDuration);
                worker.transform.DOShakeRotation(walkDuration);
                yield return new WaitForSeconds(walkDuration);
                yield return new WaitForSeconds(harvestTime);
                point.transform.localScale = Vector3.zero;

                if (!growingPlants.Contains(point))
                {
                    growingPlants.Add(point);
                }

                doneHarvestPoints.Remove(point);
                StartCoroutine(Growing(point));
            }

            worker.transform.DOMove(spawnPoint.position, walkDuration);
            worker.transform.DOShakeRotation(walkDuration);
            yield return new WaitForSeconds(walkDuration);
        }
    }

    private IEnumerator Growing(GameObject plant)
    {
        plant.transform.DOScale(endPlantScale, growTime);
        yield return new WaitForSeconds(growTime);

        if (!doneHarvestPoints.Contains(plant))
        {
            doneHarvestPoints.Add(plant);
        }

        growingPlants.Remove(plant);
    }

    private IEnumerator StopWorking()
    {
        worker.transform.DOMove(spawnPoint.position, walkDuration);
        worker.transform.DOShakeRotation(walkDuration);

        yield return new WaitForSeconds(walkDuration);
    }
}
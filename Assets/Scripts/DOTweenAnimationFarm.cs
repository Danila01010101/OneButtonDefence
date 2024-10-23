using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimationFarm : MonoBehaviour, IAnimatable
{
    [SerializeField] private GameObject Human;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private List<GameObject> DoneHarvestPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> GrowingPlants = new List<GameObject>();
    [SerializeField] private float WalkDuration;
    [SerializeField] private float HarvestTime;
    [SerializeField] private float GrowTime;

    private Coroutine currentAnimation;
    private GameObject worker;

    private void Start()
    {
        worker = Instantiate(Human, SpawnPoint.transform.position, Quaternion.identity);
    }

    public void StartAnimation() => currentAnimation = StartCoroutine(WorkerRoutine());

    public void InteruptAnimation() => StopCoroutine(currentAnimation);

    private IEnumerator WorkerRoutine()
    {
        while (true)
        {
            if (DoneHarvestPoints.Count == 0)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            int randpoint = Random.Range(0, DoneHarvestPoints.Count);
            var point = DoneHarvestPoints[randpoint];
            worker.transform.DOMove(point.transform.position, WalkDuration);
            worker.transform.DOShakeRotation(WalkDuration);
            yield return new WaitForSeconds(WalkDuration);
            yield return new WaitForSeconds(HarvestTime);
            point.transform.localScale = Vector3.zero;

            if (!GrowingPlants.Contains(point))
            {
                GrowingPlants.Add(point);
            }

            DoneHarvestPoints.Remove(point);
            StartCoroutine(Growing());
            worker.transform.DOMove(SpawnPoint.position, WalkDuration);
            worker.transform.DOShakeRotation(WalkDuration);
            yield return new WaitForSeconds(WalkDuration);
        }
    }

    private IEnumerator Growing()
    {
        for (int i = GrowingPlants.Count - 1; i >= 0; i--)
        {
            var plants = GrowingPlants[i];
            plants.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), GrowTime);
            yield return new WaitForSeconds(GrowTime);

            if (!DoneHarvestPoints.Contains(plants))
            {
                DoneHarvestPoints.Add(plants);
            }

            GrowingPlants.RemoveAt(i);
        }
    }
}
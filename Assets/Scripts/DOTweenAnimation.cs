using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimation : MonoBehaviour
{
    public GameObject Human;
    public Transform SpawnPoint;
    public List<GameObject> DoneHarvestPoints;
    public List<GameObject> GrowingPlants = new List<GameObject>();
    public float WalkDuration;
    public float HarvestTime;
    public float GrowTime;

    private GameObject worker;
    // Start is called before the first frame update
    void Start()
    {
        worker = Instantiate(Human, SpawnPoint);
        StartCoroutine(WorkerRoutine(worker));
    }

    IEnumerator WorkerRoutine(GameObject worker) 
    {
        while (true)
        {
            int randpoint = Random.Range(0, DoneHarvestPoints.Count);
            var point = DoneHarvestPoints[randpoint];
            worker.transform.DOMove(point.transform.position, WalkDuration);
            yield return new WaitForSeconds(WalkDuration);

            yield return new WaitForSeconds(HarvestTime);
            point.transform.localScale = Vector3.zero;
            DoneHarvestPoints.RemoveAt(randpoint);
            GrowingPlants.Add(point);
            StartCoroutine(Growing());

            worker.transform.DOMove(SpawnPoint.position, WalkDuration);
            yield return new WaitForSeconds(WalkDuration);
        }
    }

    IEnumerator Growing() 
    {
        for(int i = 0; i <= GrowingPlants.Count; i++)
        {
            var plants = GrowingPlants[i];
            plants.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), GrowTime);
            yield return new WaitForSeconds(GrowTime);
            GrowingPlants.Remove(plants);
            DoneHarvestPoints.Add(plants);
        }
    }
}

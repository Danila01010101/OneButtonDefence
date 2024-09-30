using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DOTweenAnimationFabric : MonoBehaviour
{
    public int WorkersCount;
    public GameObject WorkersPrefab;
    public Transform WorkerSpawnPoint;
    public float SpawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWorkers());
    }

    IEnumerator SpawnWorkers() 
    {
        for (int i = 0; i < WorkersCount; i++) 
        {
            Instantiate(WorkersPrefab, WorkerSpawnPoint);
            yield return new WaitForSeconds(SpawnDelay);
        }
        StartCoroutine(WorkersAnimation());
    }
    IEnumerator WorkersAnimation() 
    {
        yield return null;
    }
}

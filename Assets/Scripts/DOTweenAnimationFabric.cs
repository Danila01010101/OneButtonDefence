using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class DOTweenAnimationFabric : MonoBehaviour
{
    public float SpawnDelay;
    public float WalkDuration;
    public float WorkTime;
    public float LookAtDuration;
    public GameObject WorkersPrefab;
    public List<Transform> WorkerSpawnPoints;
    public List<Transform> WorkPoints;

    [SerializeField] private string Blocktag = "Coal";
    [SerializeField] private int Blockindex = 1;
    [SerializeField] private float ShakeStrenght = 90;
    [SerializeField] private int ShakeVibrato = 10;
    [SerializeField] private List<GameObject> gnomes = new List<GameObject>();

    private int WorkersCount;

    private void Start()
    {
        WorkersCount = WorkerSpawnPoints.Count;
        StartCoroutine(SpawnWorkers());
    }

    private IEnumerator SpawnWorkers()
    {
        for (int i = 0; i < WorkersCount; i++)
        {
            gnomes.Add(Instantiate(WorkersPrefab, WorkerSpawnPoints[i].position, Quaternion.identity));
            yield return new WaitForSeconds(SpawnDelay);
        }

        foreach (GameObject gnome in gnomes)
        {
            StartCoroutine(WorkersAnimation(gnome));
        }
    }

    private IEnumerator WorkersAnimation(GameObject gnome)
    {
        int index = gnomes.IndexOf(gnome);
        Vector3 workposition = WorkPoints[index].position;
        Vector3 spawnposition = WorkerSpawnPoints[index].position;
         GameObject[] test = GameObject.FindGameObjectsWithTag(Blocktag);
        var example = gnome.transform.GetChild(Blockindex);
        GameObject Block = null;

        foreach (GameObject block in test)
        {
            if(block.transform == example)
            {
                Block = block;
                break;
            }
        }

        Block.SetActive(false);

        while (true)
        {
            gnome.transform.DOLookAt(workposition, LookAtDuration);
            gnome.transform.DOShakeRotation(WalkDuration, ShakeStrenght, ShakeVibrato);
            gnome.transform.DOMove(workposition, WalkDuration);

            yield return new WaitForSeconds(WalkDuration);
            gnome.transform.DOShakeRotation(WalkDuration, ShakeStrenght, ShakeVibrato);
            yield return new WaitForSeconds(WorkTime);
            Block.SetActive(true);

            gnome.transform.DOLookAt(spawnposition, LookAtDuration);
            gnome.transform.DOShakeRotation(WalkDuration, ShakeStrenght, ShakeVibrato);
            gnome.transform.DOMove(spawnposition, WalkDuration);

            yield return new WaitForSeconds(WalkDuration);
            Block.SetActive(false);
        }
    }
}
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int spawnCount;
    [SerializeField] private float maxRandomX;
    [SerializeField] private float minRandomX;
    [SerializeField] private float maxRandomZ;
    [SerializeField] private float minRandomZ;
    [SerializeField] private float ySpawnCoordinate;
    [SerializeField] private GameObject enemyPrefab;

    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        Spawn();
    }

    void Update()
    {
        
    }

    void Spawn() 
    {
        for (int i = 0; i < spawnCount; i++) 
        {
            float x = Random.Range(minRandomX, maxRandomX);
            float z = Random.Range(minRandomZ,maxRandomZ);
            enemies.Add(Instantiate(enemyPrefab, new Vector3(gameObject.transform.position.x-x,ySpawnCoordinate,gameObject.transform.position.z-z), Quaternion.identity));
        }
    }
}

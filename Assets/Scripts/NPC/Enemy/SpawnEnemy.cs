using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public int SpawnCount;
    public float MaxRandomX;
    public float MinRandomX;
    public float MaxRandomZ;
    public float MinRandomZ;
    public float Y;
    public GameObject EnemyPref;

    private List<GameObject> enemy = new List<GameObject>();
    private float x;
    private float z;

    private void Start()
    {
        Spawn();
    }
    void Update()
    {
        
    }

    void Spawn() 
    {
        for (int i = 0; i < SpawnCount; i++) 
        {
            x = Random.Range(MinRandomX, MaxRandomX);
            z = Random.Range(MinRandomZ,MaxRandomZ);
            enemy.Add(Instantiate(EnemyPref, new Vector3(gameObject.transform.position.x-x,Y,gameObject.transform.position.z-z), Quaternion.identity));
        }
    }
}

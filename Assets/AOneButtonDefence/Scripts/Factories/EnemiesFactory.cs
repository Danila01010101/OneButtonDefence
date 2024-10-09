using System.Collections.Generic;
using UnityEngine;

public class EnemieFactory
{
    private EnemiesData data;
    private List<GameObject> enemies = new List<GameObject>();

    public EnemieFactory(EnemiesData data)
    {
        this.data = data;
        enemies = data.enemies;
    }

    public T SpawnBuilding<T>() where T : Building
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies[i];

            if (enemies is T)
            {
                GameObject spawnedBuilding = MonoBehaviour.Instantiate(enemy);
                return spawnedBuilding as T;
            }
        }

        throw new System.ArgumentException("Invalid type of building or building list is incorrect");
    }
}
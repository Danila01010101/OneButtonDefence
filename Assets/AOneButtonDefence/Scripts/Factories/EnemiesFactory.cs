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

    public T SpawnEnemy<T>(Vector3 position) where T : Building
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies[i];

            if (enemies is T)
            {
                GameObject spawnedEnemy = MonoBehaviour.Instantiate(enemy, position, Quaternion.identity);
                return spawnedEnemy as T;
            }
        }

        throw new System.ArgumentException("Invalid type of enemy or enemy list is incorrect");
    }
}
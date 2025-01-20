using System.Collections.Generic;
using UnityEngine;

public class EnemieFactory
{
    private EnemiesData data;
    private List<Knight> enemies = new List<Knight>();

    public EnemieFactory(EnemiesData data)
    {
        this.data = data;
        enemies = data.enemies;
    }

    public T SpawnEnemy<T>(Vector3 position) where T : Knight
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Knight enemy = enemies[i];

            if (enemy is T)
            {
                Knight spawnedEnemy = MonoBehaviour.Instantiate(enemy, position, Quaternion.identity);
                return spawnedEnemy as T;
            }
        }

        throw new System.ArgumentException("Invalid type of enemy or enemy list is incorrect");
    }
}
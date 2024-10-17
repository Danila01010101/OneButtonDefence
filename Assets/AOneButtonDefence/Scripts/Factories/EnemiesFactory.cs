using System.Collections.Generic;
using UnityEngine;

public class EnemieFactory
{
    private EnemiesData data;
    private List<Enemy> enemies = new List<Enemy>();

    public EnemieFactory(EnemiesData data)
    {
        this.data = data;
        enemies = data.enemies;
    }

    public T SpawnEnemy<T>(Vector3 position) where T : Enemy
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];

            if (enemies is T)
            {
                Enemy spawnedEnemy = MonoBehaviour.Instantiate(enemy, position, Quaternion.identity);
                return spawnedEnemy as T;
            }
        }

        throw new System.ArgumentException("Invalid type of enemy or enemy list is incorrect");
    }
}
using System.Collections.Generic;
using UnityEngine;

public class EnemieFactory
{
    private EnemiesData data;
    private List<FightingUnit> enemies = new List<FightingUnit>();

    public EnemieFactory(EnemiesData data)
    {
        this.data = data;
        enemies = data.enemies;
    }

    public T SpawnEnemy<T>(Vector3 position) where T : FightingUnit
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            FightingUnit enemy = enemies[i];

            if (enemy is T)
            {
                FightingUnit spawnedEnemy = MonoBehaviour.Instantiate(enemy, position, Quaternion.identity);
                return spawnedEnemy as T;
            }
        }

        throw new System.ArgumentException("Invalid type of enemy or enemy list is incorrect");
    }
}
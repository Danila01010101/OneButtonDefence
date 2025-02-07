using System.Collections.Generic;
using UnityEngine;

public class UnitsFactory
{
    private List<FightingUnit> enemies;

    public UnitsFactory(List<FightingUnit> enemies)
    {
        this.enemies = enemies;
    }

    public T SpawnUnit<T>(Vector3 position) where T : FightingUnit
    {
        FightingUnit spawnedEnemy = null;
        
        for (int i = 0; i < enemies.Count; i++)
        {
            FightingUnit enemy = enemies[i];

            if (enemy is T)
            {
                spawnedEnemy = MonoBehaviour.Instantiate(enemy, position, Quaternion.identity);
                break;
            }
        }
        
        if (spawnedEnemy== null)
            throw new System.ArgumentException("Invalid type of enemy or enemy list is incorrect");

        spawnedEnemy.Initialize();
        return spawnedEnemy as T;
    }
}
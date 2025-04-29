using System.Collections.Generic;
using UnityEngine;

public class UnitsFactory
{
    private readonly IEnemyDetector detector;
    private List<FightingUnit> enemies;

    public UnitsFactory(List<FightingUnit> enemies, IEnemyDetector detector)
    {
        this.detector = detector;
        this.enemies = enemies;
    }

    public virtual T SpawnUnit<T>(Vector3 position) where T : FightingUnit
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
        
        spawnedEnemy.Initialize(detector);
        return spawnedEnemy as T;
    }
    
    public FightingUnit SpawnSpecificUnit(FightingUnit prefab, Vector3 position)
    {
        FightingUnit spawnedEnemy = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity);
        spawnedEnemy.Initialize(detector);
        return spawnedEnemy;
    }
}
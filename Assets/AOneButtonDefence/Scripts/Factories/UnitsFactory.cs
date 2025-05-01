using System.Collections.Generic;
using UnityEngine;

public class UnitsFactory
{
    private readonly IEnemyDetector detector;
    private readonly LayerMask unitLayerMask;
    private readonly string unitTag;
    private List<FightingUnit> enemies;

    public UnitsFactory(List<FightingUnit> enemies, IEnemyDetector detector, LayerMask unitLayerMask, string unitTag)
    {
        this.detector = detector;
        this.unitLayerMask = unitLayerMask;
        this.unitTag = unitTag;
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
        SetLayer(spawnedEnemy.transform, unitLayerMask);
        spawnedEnemy.tag = unitTag;
        spawnedEnemy.Initialize(detector);
        return spawnedEnemy;
    }
    
    private void SetLayer(Transform root, LayerMask layer)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();
            current.gameObject.layer = layer;

            foreach (Transform child in current)
            {
                queue.Enqueue(child);
            }
        }
    }
}
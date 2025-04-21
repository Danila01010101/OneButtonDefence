using System;
using UnityEngine;

public class UnitWithGems : FightingUnit
{
    public static Action<Vector3, int> OnEnemyDeath;
    
    protected override void Die()
    {
        base.Die();
        int gemsAmount = UnityEngine.Random.Range(characterStats.MinGemSpawn, characterStats.MaxGemSpawn);
        EnemyDeathManager.Instance.NotifyEnemyDeath(transform.position, gemsAmount);
    }
}
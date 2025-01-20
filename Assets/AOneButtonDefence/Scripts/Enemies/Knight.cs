using System;
using UnityEngine;

public class Knight : FightingUnit
{
    [SerializeField] private Gem gemPrefab;
        
    public static Action GemSpawned;
    
    protected override void Die()
    {
        SpawnGems();
        base.Die();
    }

    private void SpawnGems()
    {
        int gemsAmount = UnityEngine.Random.Range(characterStats.MinGemSpawn, characterStats.MaxGemSpawn);

        for (int i = 0; i < gemsAmount; i++)
        {
            SpawnGem();
        }
    }

    private void SpawnGem()
    {
        Instantiate(gemPrefab, transform.position, Quaternion.identity);
        GemSpawned?.Invoke();
    }
}
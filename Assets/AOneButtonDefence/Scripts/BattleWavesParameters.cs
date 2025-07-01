using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waves data", menuName = "ScriptableObjects/NewWaveData")]
public class BattleWavesParameters : ScriptableObject
{
    [field: SerializeField] public List<WaveData> waves { get; private set; }
     
    [Serializable]
    public class EnemySpawnData
    {
        [field: SerializeField] public FightingUnit EnemyPrefab { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }

    [Serializable]
    public class WaveData
    {
        [field: SerializeField] public List<EnemySpawnData> enemiesToSpawn { get; private set; }
        [field: SerializeField] public float spawnInterval = 3;
    }
}
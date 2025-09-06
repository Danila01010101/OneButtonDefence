using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waves data", menuName = "ScriptableObjects/NewWaveData")]
public class BattleWavesParameters : ScriptableObject
{
    [field: SerializeField] public List<WaveData> waves { get; private set; }

    public enum GrowthType
    {
        Linear,
        Exponential
    }

    [Serializable]
    public class EnemyGenerationData
    {
        [field: SerializeField] public bool isEnabled = true;
        [field: SerializeField] public FightingUnit EnemyPrefab;
        [field: SerializeField] public int startWave = 1;
        [field: SerializeField] public int startAmount = 1;
        [field: SerializeField] public int growthPerWave = 1;
        [field: SerializeField] public GrowthType growthType = GrowthType.Linear;
        [field: SerializeField] public int maxWave = 100;
        [field: SerializeField] public float customSpawnInterval = -1f; // -1 = использовать волновой spawnInterval
        [field: SerializeField, Range(0f, 100f)] public float randomVariancePercent = 0f; // случайное отклонение %
    }

    [Serializable]
    public class WaveData
    {
        [field: SerializeField] public float spawnInterval = 3f;
        [field: SerializeField] public List<EnemyGenerationData> enemiesToGenerate;
    }
}
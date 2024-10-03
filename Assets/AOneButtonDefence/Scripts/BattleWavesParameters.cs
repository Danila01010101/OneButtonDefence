using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waves data", menuName = "ScriptableObjects/NewWaveData")]
public class BattleWavesParameters : ScriptableObject
{
    [field : SerializeField] private List<WaveData> waves;

    [Serializable]
    public class WaveData
    {
        [field: SerializeField] public int amountOfEnemySpawns = 3;
        [field: SerializeField] public int enemiesAmountPerSpawn = 2;
        [field: SerializeField] public float spawnInterval = 3;
    }
}
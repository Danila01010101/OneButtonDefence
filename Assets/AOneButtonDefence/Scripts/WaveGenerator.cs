using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaveGenerator
{
    public class RuntimeEnemySpawnData
    {
        public FightingUnit EnemyPrefab;
        public int Amount;
    }

    public class RuntimeWaveData
    {
        public List<RuntimeEnemySpawnData> enemiesToSpawn;
        public float spawnInterval;
    }

    public class RuntimeWavesParameters
    {
        public List<RuntimeWaveData> waves;
    }

    public static async Task<RuntimeWavesParameters> GenerateWaves(BattleWavesParameters source, int countWaves)
    {
        RuntimeWavesParameters runtime = new RuntimeWavesParameters
        {
            waves = new List<RuntimeWaveData>()
        };

        foreach (var wave in source.waves)
        {
            var runtimeWave = new RuntimeWaveData
            {
                spawnInterval = wave.spawnInterval,
                enemiesToSpawn = new List<RuntimeEnemySpawnData>()
            };

            foreach (var enemy in wave.enemiesToSpawn)
            {
                runtimeWave.enemiesToSpawn.Add(new RuntimeEnemySpawnData
                {
                    EnemyPrefab = enemy.EnemyPrefab,
                    Amount = enemy.Amount
                });
            }

            runtime.waves.Add(runtimeWave);
        }

        int lastIndex = runtime.waves.Count - 1;
        int lastEnemies = 0;
        if (runtime.waves[lastIndex].enemiesToSpawn.Count > 0)
            lastEnemies = runtime.waves[lastIndex].enemiesToSpawn[0].Amount;

        float spawnInterval = runtime.waves[lastIndex].spawnInterval;

        int startWave = runtime.waves.Count;
        int endWave = countWaves;
        int startEnemies = lastEnemies;
        int endEnemies = 130;

        float growthFactor = (float)(endEnemies - startEnemies) / (endWave - startWave);

        for (int i = startWave; i < countWaves; i++)
        {
            int enemiesAmount = Mathf.RoundToInt(startEnemies + (i - startWave) * growthFactor);

            var newWave = new RuntimeWaveData
            {
                spawnInterval = spawnInterval,
                enemiesToSpawn = new List<RuntimeEnemySpawnData>
                {
                    new RuntimeEnemySpawnData
                    {
                        EnemyPrefab = runtime.waves[lastIndex].enemiesToSpawn[0].EnemyPrefab,
                        Amount = enemiesAmount
                    }
                }
            };

            Debug.Log("New wave is " + spawnInterval + " enemies amount is " + enemiesAmount);
            runtime.waves.Add(newWave);
        }

        return runtime;
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaveGenerator
{
    private static readonly System.Random rng = new System.Random();

    public class RuntimeEnemySpawnData
    {
        public FightingUnit EnemyPrefab;
        public int Amount;
        public float SpawnInterval;
    }

    public class RuntimeWaveData
    {
        public List<RuntimeEnemySpawnData> EnemiesToSpawn;
        public float SpawnInterval;
    }

    public class RuntimeWavesParameters
    {
        public List<RuntimeWaveData> Waves;
    }

    public static Task<RuntimeWavesParameters> GenerateWaves(
        BattleWavesParameters source, int countWaves)
    {
#if UNITY_WEBGL
        // В WebGL запрещён Task.Run, возвращаем сразу
        return Task.FromResult(GenerateWavesSync(source, countWaves));
#else
        return Task.Run(() => GenerateWavesSync(source, countWaves));
#endif
    }

    private static RuntimeWavesParameters GenerateWavesSync(
        BattleWavesParameters source, int countWaves)
    {
        var runtime = new RuntimeWavesParameters
        {
            Waves = new List<RuntimeWaveData>()
        };

        for (int waveIndex = 0; waveIndex < countWaves; waveIndex++)
        {
            float waveSpawnInterval = (source.waves.Count > 0)
                ? source.waves[0].spawnInterval
                : 3f;

            var newWave = new RuntimeWaveData
            {
                SpawnInterval = waveSpawnInterval,
                EnemiesToSpawn = new List<RuntimeEnemySpawnData>()
            };

            if (source.waves.Count > 0)
            {
                foreach (var enemy in source.waves[0].enemiesToGenerate)
                {
                    if (!enemy.isEnabled) continue;
                    if (waveIndex + 1 < enemy.startWave) continue;

                    int baseAmount = 0;

                    switch (enemy.growthType)
                    {
                        case BattleWavesParameters.GrowthType.Linear:
                            baseAmount = enemy.startAmount +
                                (waveIndex + 1 - enemy.startWave) * enemy.growthPerWave;
                            break;

                        case BattleWavesParameters.GrowthType.Exponential:
                            baseAmount = Mathf.RoundToInt(
                                enemy.startAmount * Mathf.Pow(
                                    1 + enemy.growthPerWave / 100f,
                                    waveIndex + 1 - enemy.startWave));
                            break;
                    }

                    if (waveIndex + 1 > enemy.maxWave)
                        baseAmount = Math.Max(enemy.startAmount, 1);

                    baseAmount = Math.Max(baseAmount, 1);

                    if (enemy.randomVariancePercent > 0f)
                    {
                        float variance = baseAmount * (enemy.randomVariancePercent / 100f);
                        baseAmount = Mathf.RoundToInt(
                            baseAmount + RandomRange(-variance, variance));
                        baseAmount = Math.Max(baseAmount, 1);
                    }

                    float spawnInterval = (enemy.customSpawnInterval > 0)
                        ? enemy.customSpawnInterval
                        : waveSpawnInterval;

                    newWave.EnemiesToSpawn.Add(new RuntimeEnemySpawnData
                    {
                        EnemyPrefab = enemy.EnemyPrefab,
                        Amount = baseAmount,
                        SpawnInterval = spawnInterval
                    });
                }
            }

            runtime.Waves.Add(newWave);
        }

        return runtime;
    }

    private static int RandomRange(int min, int max)
    {
        lock (rng)
        {
            return rng.Next(min, max);
        }
    }

    private static float RandomRange(float min, float max)
    {
        lock (rng)
        {
            return (float)(min + (rng.NextDouble() * (max - min)));
        }
    }
}
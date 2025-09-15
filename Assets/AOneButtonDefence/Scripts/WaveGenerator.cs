using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WaveGenerator
{
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

    public static async Task<RuntimeWavesParameters> GenerateWaves(
        BattleWavesParameters source, int countWaves)
    {
        return await Task.Run(() =>
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
                                baseAmount = (int)Math.Round(
                                    enemy.startAmount * (float)Math.Pow(
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
                            baseAmount = (int)Math.Round(
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
        });
    }
    
    private static Random sysRand = new System.Random();

    private static int RandomRange(int min, int max)
    {
        return sysRand.Next(min, max);
    }

    private static float RandomRange(float min, float max)
    {
        return (float)(sysRand.NextDouble() * (max - min) + min);
    }
}
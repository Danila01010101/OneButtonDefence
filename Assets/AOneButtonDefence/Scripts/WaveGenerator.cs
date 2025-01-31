using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class WaveGenerator
{
    public static async Task<BattleWavesParameters> GenerateWaves(BattleWavesParameters wavesParameters, int countwaves)
    {
        int lastindex = wavesParameters.waves.Count - 1;
        int enemiesAmountPerSpawn = wavesParameters.waves[lastindex].enemiesAmount;
        float spawnInterval = wavesParameters.waves[lastindex].spawnInterval;
        //float spawnuppercent = 1.0003f / countwaves;
        BattleWavesParameters newWavesParameters = wavesParameters;

        for (int i = wavesParameters.waves.Count; i < countwaves; i++)
        {
            BattleWavesParameters.WaveData newWavesData = new BattleWavesParameters.WaveData();
            enemiesAmountPerSpawn = enemiesAmountPerSpawn+2;
            newWavesData.enemiesAmount = enemiesAmountPerSpawn;
            newWavesData.spawnInterval = spawnInterval;
            newWavesParameters.waves.Add(newWavesData);
        }

        //if (newWavesParameters.waves[99].amountOfEnemySpawns * newWavesParameters.waves[99].enemiesAmount * enemiesAmountPerSpawn > int.MaxValue || newWavesParameters.waves[99].amountOfEnemySpawns * newWavesParameters.waves[99].enemiesAmount * enemiesAmountPerSpawn < 0)
        //{
        //    Debug.LogError("Enemies amount too big");
        //}
        //Debug.Log($"Количество врагов на 100 волне {newWavesParameters.waves[99].amountOfEnemySpawns * newWavesParameters.waves[99].enemiesAmount}");
        Debug.Log($"Количество волн {newWavesParameters.waves.Count}");
        return newWavesParameters;
    }
}

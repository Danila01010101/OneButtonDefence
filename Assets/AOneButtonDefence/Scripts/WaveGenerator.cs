using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class WaveGenerator
{
    public static async Task<BattleWavesParameters> GenerateWaves(BattleWavesParameters wavesParameters, int countwaves)
    {
        int lastindex = wavesParameters.waves.Count - 1;
        int amountOfEnemySpawns = wavesParameters.waves[lastindex].amountOfEnemySpawns;
        int enemiesAmountPerSpawn = wavesParameters.waves[lastindex].enemiesAmountPerSpawn;
        float spawnInterval = wavesParameters.waves[lastindex].spawnInterval;
        //float spawnuppercent = 1.0003f / countwaves;
        BattleWavesParameters newWavesParameters = wavesParameters;

        for (int i = wavesParameters.waves.Count; i < countwaves; i++)
        {
            BattleWavesParameters.WaveData newWavesData = new BattleWavesParameters.WaveData();
            amountOfEnemySpawns = amountOfEnemySpawns+1;
            newWavesData.amountOfEnemySpawns = amountOfEnemySpawns;
            enemiesAmountPerSpawn = enemiesAmountPerSpawn+2;
            newWavesData.enemiesAmountPerSpawn = enemiesAmountPerSpawn;
            newWavesData.spawnInterval = spawnInterval;
            newWavesParameters.waves.Add(newWavesData);
        }

        if (newWavesParameters.waves[99].amountOfEnemySpawns * newWavesParameters.waves[99].enemiesAmountPerSpawn * enemiesAmountPerSpawn > int.MaxValue || newWavesParameters.waves[99].amountOfEnemySpawns * newWavesParameters.waves[99].enemiesAmountPerSpawn * enemiesAmountPerSpawn < 0)
        {
            Debug.LogError("Enemies amount too big");
        }
        Debug.Log($"Количество врагов на 100 волне {newWavesParameters.waves[99].amountOfEnemySpawns * newWavesParameters.waves[99].enemiesAmountPerSpawn}");
        Debug.Log($"Количество волн {newWavesParameters.waves.Count}");
        return newWavesParameters;
    }
}

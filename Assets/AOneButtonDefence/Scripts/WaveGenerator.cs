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
        float spawnuppercent = 0;
        BattleWavesParameters newWavesParameters = wavesParameters;

        for (int i = wavesParameters.waves.Count; i < countwaves; i++)
        {
            BattleWavesParameters.WaveData newWavesData = new BattleWavesParameters.WaveData();
            amountOfEnemySpawns = (int)(amountOfEnemySpawns*1.5f);
            newWavesData.amountOfEnemySpawns = amountOfEnemySpawns;
            enemiesAmountPerSpawn = (int)(enemiesAmountPerSpawn*spawnuppercent);
            newWavesData.enemiesAmountPerSpawn = enemiesAmountPerSpawn;
            newWavesData.spawnInterval = spawnInterval;
            newWavesParameters.waves.Add(newWavesData);
        }
        
        Debug.Log($"Количество волн {newWavesParameters.waves.Count}");
        return newWavesParameters;
    }
}

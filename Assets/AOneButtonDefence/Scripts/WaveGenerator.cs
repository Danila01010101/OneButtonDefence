using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class WaveGenerator
{
    public static async Task<BattleWavesParameters> GenerateWaves(BattleWavesParameters wavesParameters, int countwaves)
    {
        BattleWavesParameters newWavesParameters = wavesParameters;
        /*int lastindex = wavesParameters.waves.Count - 1;
        int enemiesAmount = wavesParameters.waves[lastindex].enemiesAmount;
        float spawnInterval = wavesParameters.waves[lastindex].spawnInterval;
        
        int startWave = wavesParameters.waves.Count;
        int endWave = countwaves;
        int startEnemies = wavesParameters.waves[startWave-1].enemiesAmount;
        int endEnemies = 130;
        float growthFactor = (float)(endEnemies - startEnemies) / (endWave - startWave); // Шаг роста

        for (int i = wavesParameters.waves.Count; i < countwaves; i++)
        {
            BattleWavesParameters.WaveData newWavesData = new BattleWavesParameters.WaveData();
            enemiesAmount = Mathf.RoundToInt(startEnemies + (i - startWave) * growthFactor);
            newWavesData.enemiesAmount = enemiesAmount;
            newWavesData.spawnInterval = spawnInterval;
            newWavesParameters.waves.Add(newWavesData);
        }

        // for (int i = 0; i < newWavesParameters.waves.Count; i++)
        // {
        //     Debug.LogWarning(newWavesParameters.waves[i].enemiesAmount);
        // }*/
        
        return newWavesParameters;
    }
}

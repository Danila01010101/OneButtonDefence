using System.Linq;
using TMPro;
using UnityEngine;

public class EnemiesCountIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesCount;

    private BattleWavesParameters wavesParameters;

    public void Initiallize(BattleWavesParameters battleWavesParameters)
    {
        wavesParameters = battleWavesParameters;
        ChangeInfo(0);
    }
    
    private void ChangeInfo(int currentWaveIndex)
    {
        int totalEnemies = 0;

        var waveData = wavesParameters.waves.ElementAtOrDefault(currentWaveIndex);
        if (waveData != null)
        {
            foreach (var enemy in waveData.enemiesToGenerate)
            {
                if (!enemy.isEnabled) continue;
                if (currentWaveIndex + 1 < enemy.startWave) continue;

                int amount = enemy.startAmount;

                if (enemy.growthType == BattleWavesParameters.GrowthType.Linear)
                    amount += (currentWaveIndex + 1 - enemy.startWave) * enemy.growthPerWave;

                if (enemy.growthType == BattleWavesParameters.GrowthType.Exponential)
                    amount = Mathf.RoundToInt(enemy.startAmount * Mathf.Pow(1 + enemy.growthPerWave / 100f, currentWaveIndex + 1 - enemy.startWave));

                if (currentWaveIndex + 1 > enemy.maxWave)
                    amount = Mathf.Max(enemy.startAmount, 1);

                totalEnemies += Mathf.Max(amount, 0);
            }
        }

        enemiesCount.text = totalEnemies.ToString();
    }

    private void Awake()
    {
        WaveCounter.Instance.OnWaveChanged += ChangeInfo;
    }

    private void OnDestroy()
    {
        WaveCounter.Instance.OnWaveChanged -= ChangeInfo;
    }
}
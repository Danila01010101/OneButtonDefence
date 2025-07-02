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
    }

    private void ChangeInfo(int currentWaveIndex) 
    {
        int nextWaveIndex = currentWaveIndex + 1;
        int totalEnemies = wavesParameters.waves
            .ElementAtOrDefault(nextWaveIndex)?
            .enemiesToSpawn
            ?.Sum(enemiesList => enemiesList.Amount)
            ?? 0;
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
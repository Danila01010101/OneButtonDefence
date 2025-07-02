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
        int totalEnemies = wavesParameters.waves
            .ElementAtOrDefault(currentWaveIndex)?
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
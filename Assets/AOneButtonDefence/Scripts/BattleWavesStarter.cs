using System.Collections;
using UnityEngine;

public class BattleWavesStarter : MonoBehaviour
{
    private BattleWavesParameters wavesParameters;

    public void Initialize(BattleWavesParameters wavesParameters)
    {
        this.wavesParameters = wavesParameters;
    }

    private void StartWave()
    {
        StartCoroutine(StartEnemiesSpawn());
    }

    private IEnumerator StartEnemiesSpawn()
    {
        yield return null;
    }
}
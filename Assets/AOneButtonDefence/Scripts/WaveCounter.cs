using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    private int currentWave = 0;
    
    public event System.Action<int> OnWaveChanged;

    public static WaveCounter Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void EndWave()
    {
        currentWave++;
        OnWaveChanged?.Invoke(currentWave);
    }
    
    public int GetCurrentWave() => currentWave;
}

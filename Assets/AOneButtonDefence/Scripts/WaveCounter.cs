using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    private static WaveCounter instance;
    private int currentWave = 0;
    
    public event System.Action<int> OnWaveChanged;
    public static WaveCounter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WaveCounter>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(WaveCounter).Name);
                    instance = singletonObject.AddComponent<WaveCounter>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void EndWave()
    {
        currentWave++;
        OnWaveChanged?.Invoke(currentWave);
    }
    
    public int GetCurrentWave() => currentWave;
}

using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathManager : MonoBehaviour
{
    private static EnemyDeathManager _instance;

    public static EnemyDeathManager Instance
    {
        get
        {
            if (_instance == null && Application.loadedLevelName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
            {
                GameObject obj = new GameObject("EnemyDeathManager");
                _instance = obj.AddComponent<EnemyDeathManager>();
            }
            return _instance;
        }
    }

    private List<IEnemyDeathListener> listeners = new List<IEnemyDeathListener>();

    public void RegisterListener(IEnemyDeathListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(IEnemyDeathListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

    public void NotifyEnemyDeath(Vector3 position, int currencyAmount)
    {
        foreach (var listener in listeners)
        {
            listener.OnEnemyDeath(position, currencyAmount);
        }
    }
}
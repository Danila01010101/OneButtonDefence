using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathManager : MonoBehaviour
{
    private static EnemyDeathManager instance;

    public static EnemyDeathManager Instance => instance;

    private List<IEnemyDeathListener> listeners = new List<IEnemyDeathListener>();

    public void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
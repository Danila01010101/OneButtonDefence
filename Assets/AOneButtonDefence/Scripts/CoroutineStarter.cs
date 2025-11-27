using System;
using UnityEngine;

public class CoroutineStarter : MonoBehaviour
{
    public static CoroutineStarter Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
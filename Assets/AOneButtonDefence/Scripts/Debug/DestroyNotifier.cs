using System;
using UnityEngine;

public class DestroyNotifier : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.Log("Object destroyed");
    }
}
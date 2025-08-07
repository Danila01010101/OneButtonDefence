using System.Collections;
using UnityEngine;

public class CoroutineStarterInitializer : MonoBehaviour, IGameComponentInitializer
{
    public IEnumerator Initialize()
    {
        var go = new GameObject("CoroutineStarter");
        go.AddComponent<CoroutineStarter>();
        go.transform.SetParent(transform);
        yield return null;
    }
}
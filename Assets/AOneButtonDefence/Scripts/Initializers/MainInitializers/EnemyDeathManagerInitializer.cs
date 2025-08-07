using System.Collections;
using UnityEngine;

public class EnemyDeathManagerInitializer : MonoBehaviour, IGameComponentInitializer
{
    public IEnumerator Initialize()
    {
        var go = new GameObject("EnemyDeathManager");
        var manager = go.AddComponent<EnemyDeathManager>();
        manager.Initialize();
        go.transform.SetParent(transform);
        yield return null;
    }
}
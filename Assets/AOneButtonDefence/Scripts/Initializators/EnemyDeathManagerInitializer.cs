using UnityEngine;
using System.Collections;

public class EnemyDeathManagerInitializer : IGameInitializerStep
{
    private Transform _parent;

    public EnemyDeathManagerInitializer(Transform parent)
    {
        _parent = parent;
    }

    public IEnumerator Initialize()
    {
        var manager = new GameObject("EnemyDeathManager").AddComponent<EnemyDeathManager>();
        manager.Initialize();
        manager.transform.SetParent(_parent);
        yield break;
    }
}
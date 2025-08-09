using UnityEngine;
using System.Collections;

public class CoroutineStarterInitializer : IGameInitializerStep
{
    private Transform _parent;

    public CoroutineStarterInitializer(Transform parent)
    {
        _parent = parent;
    }

    public IEnumerator Initialize()
    {
        var obj = new GameObject("CoroutineStarter").AddComponent<CoroutineStarter>().transform;
        obj.SetParent(_parent);
        yield break;
    }
}
using UnityEngine;
using System.Collections;

public class WaveCounterInitializer : IGameInitializerStep
{
    private Transform _parent;

    public WaveCounterInitializer(Transform parent)
    {
        _parent = parent;
    }

    public IEnumerator Initialize()
    {
        new GameObject("WaveCounter").AddComponent<WaveCounter>().transform.SetParent(_parent);
        yield break;
    }
}
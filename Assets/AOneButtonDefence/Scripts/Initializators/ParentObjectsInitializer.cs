using UnityEngine;
using System.Collections;

public class ParentObjectsInitializer : IGameInitializerStep
{
    private Transform _root;
    public Transform InitializedParent { get; private set; }

    public ParentObjectsInitializer(Transform root)
    {
        _root = root;
    }

    public IEnumerator Initialize()
    {
        InitializedParent = new GameObject("InitializedObjects").transform;
        InitializedParent.SetParent(_root);
        yield break;
    }
}
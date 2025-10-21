using UnityEngine;
using System.Collections;

public class UIObjectShowerInitializer : IGameInitializerStep
{
    private UIGameObjectShower _prefab;

    public UIObjectShowerInitializer(UIGameObjectShower prefab)
    {
        _prefab = prefab;
    }

    public IEnumerator Initialize()
    {
        Object.Instantiate(_prefab, Vector3.up * 100, Quaternion.identity);
        yield break;
    }
}
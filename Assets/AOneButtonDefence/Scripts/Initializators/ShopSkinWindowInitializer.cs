using UnityEngine;
using System.Collections;

public class ShopSkinWindowInitializer : IGameInitializerStep
{
    private SkinPanel _prefab;
    private Transform _parent;
    private IInput _input;

    public ShopSkinWindowInitializer(SkinPanel prefab, Transform parent, IInput input)
    {
        _prefab = prefab;
        _parent = parent;
        _input = input;
    }

    public IEnumerator Initialize()
    {
        var shopWindow = Object.Instantiate(_prefab, _parent);
        shopWindow.Initialize(_input);
        yield break;
    }
}
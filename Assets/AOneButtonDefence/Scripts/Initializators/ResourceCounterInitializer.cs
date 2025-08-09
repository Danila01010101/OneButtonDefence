using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceCounterInitializer : IGameInitializerStep
{
    private Transform _parent;
    private GameData _gameData;
    public GameResourcesCounter Instance { get; private set; }

    public ResourceCounterInitializer(Transform parent, GameData data)
    {
        _parent = parent;
        _gameData = data;
    }

    public IEnumerator Initialize()
    {
        Instance = new GameObject("ResourcesCounter").AddComponent<GameResourcesCounter>();
        Instance.transform.SetParent(_parent);
        var resources = new List<ResourceAmount>();
        foreach (var resource in _gameData.StartResources)
            resources.Add(new ResourceAmount(resource));
        Instance.Initialize(resources);
        yield break;
    }
}
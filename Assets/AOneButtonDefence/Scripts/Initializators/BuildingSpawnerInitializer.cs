using UnityEngine;
using System.Collections;

public class BuildingSpawnerInitializer : IGameInitializerStep
{
    private Transform _parent;
    public BuildingSpawner Instance { get; private set; }

    public BuildingSpawnerInitializer(Transform parent)
    {
        _parent = parent;
    }

    public IEnumerator Initialize()
    {
        Instance = new GameObject("BuildingSpawner").AddComponent<BuildingSpawner>();
        Instance.transform.SetParent(_parent);
        yield break;
    }
}
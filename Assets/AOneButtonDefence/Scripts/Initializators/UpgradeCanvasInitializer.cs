using UnityEngine;
using System.Collections;

public class UpgradeCanvasInitializer : IGameInitializerStep
{
    private GameplayCanvas _prefab;
    private WorldGenerationData _data;
    public GameplayCanvas CanvasInstance { get; private set; }

    public UpgradeCanvasInitializer(GameplayCanvas prefab, WorldGenerationData data)
    {
        _prefab = prefab;
        _data = data;
    }

    public IEnumerator Initialize()
    {
        CanvasInstance = Object.Instantiate(_prefab);
        CanvasInstance.Initialize(_data.BuildingsData.Buildings.Count, _data.BuildingsData);
        yield break;
    }
}
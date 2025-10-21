using UnityEngine;
using System.Collections;

public class WorldGridInitializer : IGameInitializerStep
{
    private Transform _parent;
    private WorldGenerationData _data;
    private BuildingSpawner _spawner;
    private MonoBehaviour _mono;
    public GroundBlocksSpawner WorldCreator { get; private set; }
    public CellsGrid Grid { get; private set; }

    public WorldGridInitializer(Transform parent, WorldGenerationData data, BuildingSpawner spawner, MonoBehaviour mono)
    {
        _parent = parent;
        _data = data;
        _spawner = spawner;
        _mono = mono;
    }

    public IEnumerator Initialize()
    {
        WorldCreator = new GameObject("WorldCreator").AddComponent<GroundBlocksSpawner>();
        WorldCreator.transform.SetParent(_parent);
        Grid = new CellsGrid(_data.GridSize, _data.CellsInterval);
        WorldCreator.SetupGrid(_data, Grid, _spawner, _mono);
        yield break;
    }

    public bool IsWorldReady() => WorldCreator != null && WorldCreator.IsWorldReady;
}
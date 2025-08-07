using System.Collections;
using UnityEngine;

public class WorldInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private WorldGenerationData worldGenData;
    [SerializeField] private GameData gameData;

    public static GroundBlocksSpawner WorldCreator { get; private set; }
    public static CellsGrid Grid { get; private set; }
    public static BuildingSpawner Spawner { get; private set; }

    public IEnumerator Initialize()
    {
        Spawner = new GameObject("BuildingSpawner").AddComponent<BuildingSpawner>();
        Spawner.transform.SetParent(transform);

        WorldCreator = new GameObject("WorldCreator").AddComponent<GroundBlocksSpawner>();
        WorldCreator.transform.SetParent(transform);

        Grid = new CellsGrid(worldGenData.GridSize, worldGenData.CellsInterval);
        WorldCreator.SetupGrid(worldGenData, Grid, Spawner, this);

        yield return new WaitUntil(() => WorldCreator.IsWorldReady);

        Spawner.Initialize(Grid, worldGenData.BuildingsData, gameData.UpgradeStateDuration);
        yield return null;
    }
}
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private GroundBlocksSpawner worldCreator;
    [SerializeField] private BuildingSpawner changer;
    [SerializeField] private PartMenadger partManagerPrefab;

    private GameStateMachine gameStateMachine;

    private void Awake()
    {
        new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();
        var newGrid = new CellsGrid(worldGenerationData.GridSize, worldGenerationData.CellsInterval);
        worldCreator.SetupGrid(newGrid, changer);
        Instantiate(partManagerPrefab).Initialize(worldGenerationData.startButtonsAmount);
        gameStateMachine = new GameStateMachine(worldCreator, gameData);
    }
}
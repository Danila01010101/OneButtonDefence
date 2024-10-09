using UnityEngine;
using static GameStateMachine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private WorldGenerationData worldGenerationData;
    [SerializeField] private GroundBlocksSpawner worldCreator;
    [SerializeField] private BuildingSpawner changer;
    [SerializeField] private PartManager partManagerPrefab;

    private GameStateMachine gameStateMachine;

    private void Awake()
    {
        new GameObject("ResourcesCounter").AddComponent<ResourcesCounter>();
        var newGrid = new CellsGrid(worldGenerationData.GridSize, worldGenerationData.CellsInterval);
        worldCreator.SetupGrid(newGrid, changer);
        PartManager upgradeCanvas = Instantiate(partManagerPrefab);
        upgradeCanvas.Initialize(worldGenerationData.startButtonsAmount);
        GameStateMachineData gameStateMachineData = new GameStateMachineData
        (
            upgradeCanvas.gameObject,
            gameData,
            worldCreator
        );
        gameStateMachine = new GameStateMachine(gameStateMachineData);
    }
}
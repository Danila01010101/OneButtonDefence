using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlocksSpawner : MonoBehaviour
{
    public bool IsWorldReady { get; private set; } = false;

    [SerializeField] private WorldGenerationData data;

    private List<List<Ground>> groundBlocks = new List<List<Ground>>();
    private BuildingSpawner buildingSpawner;
    private Transform blocksParent;
    private CellsGrid grid;

    public void SetupGrid(CellsGrid grid, BuildingSpawner buildingSpawner, MonoBehaviour coroutineRunner)
    {
        this.grid = grid;
        this.buildingSpawner = buildingSpawner;
        buildingSpawner.CellFilled += ReplaceBlockWithDefaultBlock;
        coroutineRunner.StartCoroutine(GenerateWorld(buildingSpawner));
    }

    private IEnumerator GenerateWorld(BuildingSpawner cellsChanger)
    {
        blocksParent = new GameObject("Earth blocks").transform;

        for (int i = 0; i < grid.Size; i++)
        {
            groundBlocks.Add(new List<Ground>());
            
            for (int j = 0; j < grid.Size; j++)
            {
                Ground spawnedBlock = SpawnBlock(GetRandomEarthBlock(), i, j);
                groundBlocks[i].Add(spawnedBlock);
            }
            
            yield return null;
        }

        SpawnStartCamp();
        IsWorldReady = true;
    }

    private void SpawnStartCamp()
    {
        CellPlaceCoordinates placePosition = grid.GetBestCellCoordinates();
        Destroy(groundBlocks[placePosition.X][placePosition.Z].gameObject);
        SpawnBlock(data.CentralBlock, placePosition.X, placePosition.Z);
        grid.Place(placePosition);
    }

    private Ground SpawnBlock(Ground block, int xCoordinate, int zCoordinate)
    {
        Ground spawnedBlock = Instantiate(block, grid.GetWorldPositionByCoordinates(xCoordinate, zCoordinate), Quaternion.identity);
        spawnedBlock.gameObject.transform.SetParent(blocksParent);
        return spawnedBlock;
    }

    private Ground GetRandomEarthBlock()
    {
        int blockIndex = Random.Range(0, data.EarthBlocks.Count);

        return data.EarthBlocks[blockIndex];
    }

    private void ReplaceBlockWithDefaultBlock(CellPlaceCoordinates placePosition)
    {
        Ground replacedBlock = groundBlocks[placePosition.X][placePosition.Z];
        replacedBlock.ActivateBonus();
        Destroy(replacedBlock.gameObject);
        SpawnBlock(data.EmptyBlock, placePosition.X, placePosition.Z);
    }

    private void OnDestroy()
    {
        buildingSpawner.CellFilled -= ReplaceBlockWithDefaultBlock;
    }
}
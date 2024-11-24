using System.Collections.Generic;
using UnityEngine;

public class GroundBlocksSpawner
{
    private List<List<Ground>> spawnedGroundBlocks = new List<List<Ground>>();
    private GroundBlocksFactory groundBlocksFactory;
    private BuildingSpawner buildingSpawner;
    private Transform blocksParent;
    private CellsGrid grid;

    public void SetupGrid(CellsGrid grid, BuildingSpawner buildingSpawner, GroundBlocksFactory blocksFactory)
    {
        this.grid = grid;
        this.buildingSpawner = buildingSpawner;
        this.groundBlocksFactory = blocksFactory;
        buildingSpawner.CellFilled += ReplaceBlockWithDefaultBlock;
    }

    public void GenerateNewWorld()
    {
        blocksParent = new GameObject("Earth blocks").transform;

        for (int i = 0; i < grid.Size; i++)
        {
            spawnedGroundBlocks.Add(new List<Ground>());
            for (int j = 0; j < grid.Size; j++)
            {
                Ground spawnedBlock = SpawnBlock(groundBlocksFactory.GetRandomEarthBlock(), i, j);
                spawnedBlock.Initialize();
                spawnedGroundBlocks[i].Add(spawnedBlock);
                grid.SetGround(new CellPlaceCoordinates(i, j), spawnedBlock);
            }
        }

        SpawnMainBuilding();
    }

    public void GenerateExistingWorld()
    {
        blocksParent = new GameObject("Earth blocks").transform;

        for (int i = 0; i < grid.Size; i++)
        {
            spawnedGroundBlocks.Add(new List<Ground>());
            for (int j = 0; j < grid.Size; j++)
            {
                Ground blockPrefab = groundBlocksFactory.GetBlockByType(grid.PlacementGrid[i][j].GroundBlock);
                Ground spawnedBlock = SpawnBlock(blockPrefab, i, j);
                spawnedBlock.Initialize();
                spawnedGroundBlocks[i].Add(spawnedBlock);
                grid.SetGround(new CellPlaceCoordinates(i, j), spawnedBlock);
            }
        }

        SpawnMainBuilding();
    }

    private void SpawnMainBuilding()
    {
        CellPlaceCoordinates placePosition = grid.GetBestCellCoordinates();
        MonoBehaviour.Destroy(spawnedGroundBlocks[placePosition.X][placePosition.Z].gameObject);
        Ground block = SpawnBlock(groundBlocksFactory.GetCentralBlock(), placePosition.X, placePosition.Z);
        grid.SetGround(placePosition, block);
    }

    private Ground SpawnBlock(Ground block, int xCoordinate, int zCoordinate)
    {
        Ground spawnedBlock = MonoBehaviour.Instantiate(block, grid.GetWorldPositionByCoordinates(xCoordinate, zCoordinate), Quaternion.identity);
        spawnedBlock.gameObject.transform.SetParent(blocksParent);
        return spawnedBlock;
    }

    private void ReplaceBlockWithDefaultBlock(CellPlaceCoordinates placePosition)
    {
        Ground replacedBlock = spawnedGroundBlocks[placePosition.X][placePosition.Z];
        replacedBlock.ActivateBonus();
        MonoBehaviour.Destroy(replacedBlock.gameObject);
        SpawnBlock(groundBlocksFactory.GetEmptyBlock(), placePosition.X, placePosition.Z);
    }

    private void OnDestroy()
    {
        buildingSpawner.CellFilled -= ReplaceBlockWithDefaultBlock;
    }
}
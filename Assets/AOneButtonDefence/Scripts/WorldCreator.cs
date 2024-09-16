using System.Collections.Generic;
using UnityEngine;

public class WorldCreator : MonoBehaviour
{
    [SerializeField] private GameData data;

    private List<List<Ground>> groundBlocks = new List<List<Ground>>();
    private Transform blocksParent;
    private CellsGrid grid;

    public void SetupGrid(CellsGrid grid, GridChanger cellsChanger)
    {
        this.grid = grid;
        GenerateWorld(cellsChanger);
    }

    private void GenerateWorld(GridChanger cellsChanger)
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
        }

        SpawnStartCamp();
        cellsChanger.Initialize(grid, data.Buildings);
    }

    private void SpawnStartCamp()
    {
        CellPlacePosition placePosition = grid.GetBestCellPlace();
        Destroy(groundBlocks[placePosition.X][placePosition.Z].gameObject);
        SpawnBlock(data.CentralBlock, placePosition.X, placePosition.Z);
    }

    private Ground SpawnBlock(Ground block, int xCoordinate, int zCoordinate)
    {
        Ground spawnedBlock = Instantiate(block, GetVectorByCoordinates(xCoordinate, zCoordinate), Quaternion.identity);
        spawnedBlock.gameObject.transform.SetParent(blocksParent);
        return spawnedBlock;
    }

    private Vector3 GetVectorByCoordinates(int xCoordinate, int zCoordinate) => 
        new Vector3(xCoordinate * data.CellSize, 0, zCoordinate * data.CellSize);

    private Ground GetRandomEarthBlock()
    {
        int blockIndex = Random.Range(0, data.EarthBlocks.Count);

        return data.EarthBlocks[blockIndex];
    }
}
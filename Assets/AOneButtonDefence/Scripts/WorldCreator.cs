using UnityEngine;

public class WorldCreator : MonoBehaviour
{
    [SerializeField] private GameData data;

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
            for (int j = 0; j < grid.Size; j++)
            {
                var position = new Vector3 (i * data.CellSize, 0, j * data.CellSize);
                var spawnedBlock = Instantiate(GetRandomEarthBlock(), position, Quaternion.identity);
                spawnedBlock.transform.SetParent(blocksParent);
            }
        }

        cellsChanger.Initialize(grid);
    }

    private GameObject GetRandomEarthBlock()
    {
        int blockIndex = Random.Range(0, data.EarthBlocks.Count);

        return data.EarthBlocks[blockIndex];
    }
}
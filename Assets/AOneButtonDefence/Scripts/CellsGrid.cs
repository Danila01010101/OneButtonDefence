using System.Collections.Generic;
using UnityEngine;

public class CellsGrid
{
    public int Size { get; private set; }

    private float cellsSpawnDistance;
    private List<List<bool>> placementGrid = new List<List<bool>>();
    private CellPlacePosition centerPosition = new CellPlacePosition(49, 49);

    public CellsGrid(int gridSize, float cellsSpawnDistance)
    {
        this.Size = gridSize;
        this.cellsSpawnDistance = cellsSpawnDistance;

        for (int i = 0; i < gridSize; i++)
        {
            var newRow = new List<bool>();

            for (int j = 0; j < gridSize; j++)
            {
                newRow.Add(false);
            }

            placementGrid.Add(newRow);
        }
    }

    public void Place(CellPlacePosition placePosition) 
    {
        if (IsPlaceBusy(placePosition) == false)
        {
            placementGrid[placePosition.X][placePosition.Z] = true;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException("You can`t place cell here. This place is already taken.");
        }
    }

    public CellPlacePosition GetBestCellPlace()
    {
        CellPlacePosition newCellPlace = centerPosition;

        while (IsPlaceBusy(newCellPlace))
        {
            newCellPlace = GetFurtherCellPosition(newCellPlace);
        }

        Debug.Log(string.Format("Best place position is {0} : {1}", newCellPlace.X, newCellPlace.Z));
        return newCellPlace;
    }

    public Vector3 GetWorldPositionByCoordinates(int xCoordinate, int zCoordinate) =>
        new Vector3(xCoordinate * cellsSpawnDistance, 0, zCoordinate * cellsSpawnDistance);

    private bool IsPlaceBusy(CellPlacePosition position) => placementGrid[position.X][position.Z];

    private CellPlacePosition GetFurtherCellPosition(CellPlacePosition position)
    {
        int checkDistance = 1;
        List<CellPlacePosition> cellsAround = GetSurroundingCells(position, checkDistance);
        List<CellPlacePosition> checkedCells = new List<CellPlacePosition>();
        int randomIndex = Random.Range(0, cellsAround.Count);
        CellPlacePosition furtherCellPosition = cellsAround[randomIndex];

        while (IsPlaceBusy(furtherCellPosition))
        {
            checkedCells.Add(furtherCellPosition);

            if (checkedCells.Count == cellsAround.Count)
            {
                checkDistance++;
                cellsAround = GetSurroundingCells(position, checkDistance);
            }

            randomIndex = Random.Range(0, cellsAround.Count);
            furtherCellPosition = cellsAround[randomIndex];
        }

        return furtherCellPosition;
    }

    private List<CellPlacePosition> GetSurroundingCells(CellPlacePosition position, int distance)
    {
        List<CellPlacePosition> elements = new List<CellPlacePosition>();

        int rows = placementGrid.Count;
        int cols = placementGrid[0].Count;
        int x = position.X;
        int z = position.Z;

        for (int i = x - distance; i <= x + distance; i++)
        {
            for (int j = z - distance; j <= z + distance; j++)
            {
                if (i >= 0 && i < rows && j >= 0 && j < cols && (i != x || j != z))
                {
                    elements.Add(new CellPlacePosition(i, j));
                }
            }
        }

        return elements;
    }
}
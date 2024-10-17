using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CellsGrid
{
    public int Size { get; private set; }

    private float cellsSpawnDistance;
    private List<List<bool>> placementGrid = new List<List<bool>>();
    private CellPlaceCoordinates centerPosition = new CellPlaceCoordinates(49, 49);

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

    public void Place(CellPlaceCoordinates placePosition) 
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

    public CellPlaceCoordinates GetBestCellCoordinates()
    {
        CellPlaceCoordinates newCellPlace = centerPosition;

        while (IsPlaceBusy(newCellPlace))
        {
            newCellPlace = GetFurtherCellPosition(newCellPlace);
        }

        return newCellPlace;
    }

    public Vector3 GetBestCellPosition()
    {
        CellPlaceCoordinates cellPlace = GetBestCellCoordinates();
        return GetWorldPositionByCoordinates(cellPlace.X, cellPlace.Z);
    }

    public Vector3 GetWorldPositionByCoordinates(int xCoordinate, int zCoordinate) =>
        new Vector3(xCoordinate * cellsSpawnDistance, 0, zCoordinate * cellsSpawnDistance);

    private bool IsPlaceBusy(CellPlaceCoordinates position) => placementGrid[position.X][position.Z];

    private CellPlaceCoordinates GetFurtherCellPosition(CellPlaceCoordinates position)
    {
        int checkDistance = 1;
        List<CellPlaceCoordinates> cellsAround = GetSurroundingCells(position, checkDistance);
        List<CellPlaceCoordinates> checkedCells = new List<CellPlaceCoordinates>();
        int randomIndex = Random.Range(0, cellsAround.Count);
        CellPlaceCoordinates furtherCellPosition = cellsAround[randomIndex];

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

    private List<CellPlaceCoordinates> GetSurroundingCells(CellPlaceCoordinates position, int distance)
    {
        List<CellPlaceCoordinates> elements = new List<CellPlaceCoordinates>();

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
                    elements.Add(new CellPlaceCoordinates(i, j));
                }
            }
        }

        return elements;
    }
}
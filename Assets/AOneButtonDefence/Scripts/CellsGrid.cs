using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CellsGrid
{
    public int Size { get; private set; }

    public List<List<CellBlockInfo>> PlacementGrid => placementGrid;

    private float cellsSpawnDistance;
    private List<List<CellBlockInfo>> placementGrid = new List<List<CellBlockInfo>>();
    private CellPlaceCoordinates centerPosition = new CellPlaceCoordinates(49, 49);

    public CellsGrid(int gridSize, float cellsSpawnDistance)
    {
        this.Size = gridSize;
        this.cellsSpawnDistance = cellsSpawnDistance;

        for (int i = 0; i < gridSize; i++)
        {
            var newRow = new List<CellBlockInfo>();

            for (int j = 0; j < gridSize; j++)
            {
                newRow.Add(new CellBlockInfo(new CellPlaceCoordinates(i, j)));
            }

            placementGrid.Add(newRow);
        }
    }

    public void PlaceBuilding(CellPlaceCoordinates placeCoordinates, Building.BuildingType type)
    {
        CellBlockInfo cellBlockInfo = placementGrid[placeCoordinates.X][placeCoordinates.Z];

        if (IsBuildingInPlace(placeCoordinates) == false)
        {
            cellBlockInfo.SetBuildingBlock(type);
        }
        else
        {
            throw new System.ArgumentOutOfRangeException("You can`t place cell here. This place is already taken.");
        }
    }

    public void SetGround(CellPlaceCoordinates placeCoordinates, Ground block) => placementGrid[placeCoordinates.X][placeCoordinates.Z].SetGroundBlock(block.Type);

    public CellPlaceCoordinates GetBestCellCoordinates()
    {
        CellPlaceCoordinates newCellPlace = centerPosition;

        while (IsBuildingInPlace(newCellPlace))
        {
            newCellPlace = GetFurtherCellPosition(newCellPlace, 0);
        }

        return newCellPlace;
    }

    public Vector3 GetRandomEmptyCellPosition(int spread)
    {
        CellPlaceCoordinates cellPlace = GetFurtherCellPosition(centerPosition, spread);
        return GetWorldPositionByCoordinates(cellPlace.X, cellPlace.Z);
    }

    public Vector3 GetWorldPositionByCoordinates(int xCoordinate, int zCoordinate) =>
        new Vector3(xCoordinate * cellsSpawnDistance, 0, zCoordinate * cellsSpawnDistance);

    private bool IsBuildingInPlace(CellPlaceCoordinates position) => placementGrid[position.X][position.Z].IsBuildingPlaced;

    private CellPlaceCoordinates GetFurtherCellPosition(CellPlaceCoordinates position, int spread)
    {
        int minCheckDistance = 1;
        List<CellPlaceCoordinates> cellsAround = GetSurroundingCells(position, minCheckDistance);
        List<CellPlaceCoordinates> checkedCells = new List<CellPlaceCoordinates>();
        int randomIndex = UnityEngine.Random.Range(0, cellsAround.Count);
        CellPlaceCoordinates furtherCellPosition = cellsAround[randomIndex];

        while (IsBuildingInPlace(furtherCellPosition))
        {
            checkedCells.Add(furtherCellPosition);

            if (checkedCells.Count == cellsAround.Count)
            {
                minCheckDistance++;
                cellsAround = GetSurroundingCells(position, minCheckDistance);
            }

            randomIndex = UnityEngine.Random.Range(0, cellsAround.Count);
            furtherCellPosition = cellsAround[randomIndex];
        }

        if (spread > 0)
        {
            int maxCheckDistance = minCheckDistance + spread;
            cellsAround = GetSurroundingCells(position, maxCheckDistance);

            foreach (CellPlaceCoordinates cell in checkedCells)
            {
                cellsAround.Remove(cell);
            }

            randomIndex = UnityEngine.Random.Range(0, cellsAround.Count);
            return cellsAround[randomIndex];
        }
        else
        {
            return furtherCellPosition;
        }
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
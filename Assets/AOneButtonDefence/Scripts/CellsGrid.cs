using System.Collections.Generic;
using UnityEngine;

public class CellsGrid : MonoBehaviour
{
    private int gridSize;

    private List<List<bool>> placementGrid = new List<List<bool>>();

    public void Initialize(int gridSize)
    {
        this.gridSize = gridSize;

        for (int i = gridSize; i < gridSize; i++)
        {
            var newRow = new List<bool>(gridSize);
            placementGrid.Add(newRow);
        }
    }

    public void Place(CellPlacePosition placePosition) 
    {
        if (IsPlaceBusy(placePosition) == false)
        {
            placementGrid[placePosition.X][placePosition.Y] = true;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException("You can`t place cell here. This place is already taken.");
        }
    }

    public CellPlacePosition GetBestCellPlace()
    {
        CellPlacePosition newCellPlace = new CellPlacePosition(0, 0);

        while (IsPlaceBusy(newCellPlace))
        {
            newCellPlace = GetFurtherCellPosition(newCellPlace);
        }

        return newCellPlace;
    }

    private bool IsPlaceBusy(CellPlacePosition position)
    {
        CellPlacePosition centeredPosition = ConvertToCenteredPosition(position);

        return placementGrid[centeredPosition.X][centeredPosition.Y];
    }

    private CellPlacePosition GetFurtherCellPosition(CellPlacePosition position)
    {
        if (position.X > position.Y)
        {
            return new CellPlacePosition(position.X, position.Y + 1);
        }
        else
        {
            return new CellPlacePosition(position.X + 1, position.Y);
        }
    }

    private CellPlacePosition ConvertToCenteredPosition(CellPlacePosition position) => new CellPlacePosition(position.X + gridSize / 2, position.Y + gridSize / 2);
}
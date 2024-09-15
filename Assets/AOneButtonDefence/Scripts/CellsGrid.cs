using System.Collections.Generic;

public class CellsGrid
{
    public int Size { get; private set; }

    private List<List<bool>> placementGrid = new List<List<bool>>();

    public CellsGrid(int gridSize)
    {
        this.Size = gridSize;

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
        return placementGrid[position.X][position.Y];
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

    private CellPlacePosition ConvertToCenteredPosition(CellPlacePosition position) => new CellPlacePosition(position.X + Size / 2, position.Y + Size / 2);
}
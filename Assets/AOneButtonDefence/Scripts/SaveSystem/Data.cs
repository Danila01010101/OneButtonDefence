using System;

[Serializable]
public class Data
{
    public CellsGrid CellsGrid;
    public int CurrentLevel = 1;

    public void InitializeEmptyData(CellsGrid cellsGrid) 
    {
        this.CellsGrid = cellsGrid;
    }
}
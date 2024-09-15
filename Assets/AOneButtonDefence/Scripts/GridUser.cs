using UnityEngine;

public class GridUser : MonoBehaviour
{
    protected CellsGrid grid { get; private set; }

    public void SetupGrid(CellsGrid grid)
    {
        this.grid = grid;
    }
}
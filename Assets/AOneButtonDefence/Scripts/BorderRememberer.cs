using UnityEngine;

public class BorderRememberer : MonoBehaviour
{
    private float cellSize;
    private int outOfTownCoordinate;
    private float spawnSpread;

    public static BorderRememberer Instance { get; private set; }

    private void Start()
    {
        if (Instance != this)
            Destroy(Instance.gameObject);

        Instance = this;
    }

    public void Initialize(ICellPlacer cellPlacer, float cellSize, float spawnSpread)
    {
        this.cellSize = cellSize;
        cellPlacer.CellFilled += UpdateFurtherCellPosition;
        this.spawnSpread = spawnSpread;
    }

    private void UpdateFurtherCellPosition(CellPlacePosition placePosition) => outOfTownCoordinate = placePosition.X > placePosition.Z ? placePosition.Z : placePosition.X;

    public Vector3 GetOutBorderPlaceToSpawn()
    {
        float outOfTownPosition = outOfTownCoordinate * cellSize;
        float x = Random.Range(outOfTownPosition, outOfTownPosition + spawnSpread);
        float z = Random.Range(outOfTownPosition, outOfTownPosition + spawnSpread);

        return new Vector3(x, z);
    }
}
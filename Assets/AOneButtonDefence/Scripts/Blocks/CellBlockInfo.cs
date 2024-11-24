using System;

[Serializable]
public class CellBlockInfo
{
    public Ground.GroundBlockType GroundBlock { get; private set; } = Ground.GroundBlockType.Empty;
    public Building.BuildingType BuildingBlock { get; private set; } = Building.BuildingType.Empty;
    public CellPlaceCoordinates Position { get; private set; }
    public bool IsBuildingPlaced => BuildingBlock != Building.BuildingType.Empty;
    public bool IsGroundPlaced => GroundBlock == Ground.GroundBlockType.Empty;

    public CellBlockInfo(CellPlaceCoordinates placeCoordinates)
    {
        Position = placeCoordinates;
    }

    public void SetCoordinatePosition(CellPlaceCoordinates coordinates)
    {

    }

    public void SetGroundBlock(Ground.GroundBlockType groundBlockType) => GroundBlock = groundBlockType;

    public void SetBuildingBlock(Building.BuildingType type) => BuildingBlock = type;
}
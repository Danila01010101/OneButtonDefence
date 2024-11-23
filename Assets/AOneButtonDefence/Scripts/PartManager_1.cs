using UnityEngine;

public partial class PartManager : MonoBehaviour
{
    private int ConvertUpgradeToInt(Building.BuildingType type)
    {
        switch (type)
        {
            case Building.BuildingType.Empty:
                return 0;
            case Building.BuildingType.Farm:
                return 1;
            case Building.BuildingType.Factory:
                return 2;
            case Building.BuildingType.MilitaryCamp:
                return 3;
            case Building.BuildingType.SpiritBuilding:
                return 4;
            default:
                throw new System.ArgumentException("Incorrect building type.");
        }
    }

    private Building.BuildingType ConvertIntToUpgrade(int index)
    {
        switch (index)
        {
            case 0:
                return Building.BuildingType.Empty;
            case 1:
                return Building.BuildingType.Farm;
            case 2:
                return Building.BuildingType.Factory;
            case 3:
                return Building.BuildingType.MilitaryCamp;
            case 4:
                return Building.BuildingType.SpiritBuilding;
            default:
                throw new System.ArgumentException("Incorrect building type.");
        }
    }
}
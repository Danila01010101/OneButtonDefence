using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "ScriptableObjects/New Upgrades Data")]
public class BuildingsData : ScriptableObject
{
    [field : SerializeField] public BasicBuildingData FarmData { get; private set; }
    [field : SerializeField] public BasicBuildingData MilitaryCampData { get; private set; }
    [field : SerializeField] public BasicBuildingData FactoryData { get; private set; }
    [field : SerializeField] public BasicBuildingData SpiritBuildingData { get; private set; }
}
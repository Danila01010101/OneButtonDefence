using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "ScriptableObjects/New Upgrades Data")]
public class BuildingsData : ScriptableObject
{
    [field : SerializeField] public List<Building> buildingsList { get; private set; }
    [field : SerializeField] public FarmData FarmData { get; private set; }
    [field : SerializeField] public MilitaryCampData MilitaryCampData { get; private set; }
    [field : SerializeField] public FactoryData FactoryData { get; private set; }
    [field : SerializeField] public SpiritBuildingData SpiritBuildingData { get; private set; }
}
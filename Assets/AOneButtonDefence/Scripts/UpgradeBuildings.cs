using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "ScriptableObjects/New Upgrades Data")]
public class UpgradeBuildings : ScriptableObject
{
    [field : SerializeField] public Factory Factory { get; private set; }
    [field : SerializeField] public Farm Farm { get; private set; }
    [field : SerializeField] public MilitaryCamp MilitaryCamp { get; private set; }
    [field : SerializeField] public SpiritBuilding SpiritBuilding { get; private set; }
}
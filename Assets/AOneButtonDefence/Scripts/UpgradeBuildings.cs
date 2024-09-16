using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "ScriptableObjects/New Upgrades Data")]
public class UpgradeBuildings : ScriptableObject
{
    [field: SerializeField] public List<Building> buildings { get; private set; }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "ScriptableObjects/New Upgrades Data")]
public class BuildingsData : ScriptableObject
{
    [field : SerializeField] public List<BasicBuildingData> Buildings { get; private set; }
}
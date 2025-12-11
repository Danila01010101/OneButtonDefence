using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReviveResourcesData", menuName = "ScriptableObjects/New Revive Resources Data")]
public class ReviveResourcesData : ScriptableObject
{
    [field: SerializeField] public int WarriorsPerBuildingBonus { get; private set; }
    [field: SerializeField] public ResourceData WarriorsResource { get; private set; }
    [field: SerializeField] public List<StartResourceAmount> ReviveResources { get; private set; }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsData", menuName = "ScriptableObjects/New Buildings Data")]
public class BuildingsData : ScriptableObject
{
    [field : SerializeField] public List<BasicBuildingData> Buildings { get; private set; }
}
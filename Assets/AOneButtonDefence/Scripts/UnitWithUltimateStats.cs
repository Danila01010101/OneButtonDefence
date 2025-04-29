using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitWithUltimateStats", menuName = "ScriptableObjects/New Ultimate Stats")]
public class UnitWithUltimateStats : ScriptableObject
{
    [field : SerializeField] public List<SpellData> SpellsData { get; private set; }
    [field : SerializeField] public float UltimateDelay { get; private set; } = 1;
}
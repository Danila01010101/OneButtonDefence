using System.Collections.Generic;
using UnityEngine;

namespace AOneButtonDefence.Scripts
{
    public class UnitWithUltimateStats : ScriptableObject
    {
        [field: SerializeField] public CharacterStats BaseStats { get; private set; }
        [field : SerializeField] public List<SpellData> SpellsData { get; private set; }
        [field : SerializeField] public float UltimateDelay { get; private set; } = 1;
    }
}
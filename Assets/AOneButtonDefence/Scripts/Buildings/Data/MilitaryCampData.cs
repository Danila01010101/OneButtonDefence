using System;
using UnityEngine;

[Serializable]
public class MilitaryCampData : BasicBuildingData
{
    [field: SerializeField] public FightingUnit GnomeWarriorPrefab { get; private set; }
    [field: SerializeField] public int StartWarriorsAmount { get; private set; }
    [field: SerializeField] public int EveryTurnWarriorsAmount { get; private set; }
}
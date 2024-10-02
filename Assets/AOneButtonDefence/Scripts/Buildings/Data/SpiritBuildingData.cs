using System;
using UnityEngine;

[Serializable]
public class SpiritBuildingData : BasicBuildingData
{
    [field: SerializeField] public int SpawnBonus { get; private set; }
    [field: SerializeField] public int EveryTurnBonus { get; private set; }
}
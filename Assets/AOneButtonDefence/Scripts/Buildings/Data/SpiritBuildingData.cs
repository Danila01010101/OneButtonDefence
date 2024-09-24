using System;
using UnityEngine;

[Serializable]
public class SpiritBuildingData
{
    [field: SerializeField] public int SpawnBonus { get; private set; }
    [field: SerializeField] public int EveryTurnBonus { get; private set; }
}
using System;
using UnityEngine;

[Serializable]
public class FarmData
{
    [field: SerializeField] public int SpawnBonus { get; private set; }
    [field: SerializeField] public int EveryTurnBonus { get; private set; }
    [field: SerializeField] public int StartHumanAmount { get; private set; }
}
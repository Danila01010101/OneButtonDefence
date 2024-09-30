using System;
using UnityEngine;

[Serializable]
public class MilitaryCampData
{
    [field: SerializeField] public int SpawnWarriorsAmount { get; private set; }
    [field: SerializeField] public int EveryTurnWarriorsAmount { get; private set; }
}
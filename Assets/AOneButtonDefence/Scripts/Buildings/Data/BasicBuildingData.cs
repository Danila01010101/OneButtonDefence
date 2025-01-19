using System;
using UnityEngine;

[Serializable]
public abstract class BasicBuildingData
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int FoodPerTurnAmount { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
}
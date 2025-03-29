using System;
using UnityEngine;

[Serializable]
public abstract class BasicBuildingData 
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Building Prefab { get; private set; }

    [Header("Стоимость постройки")]
    public ResourceChangeData[] buildResourceConsumption;
    public ResourceChangeData[] buildResourceIncome;

    [Header("Доход/расход ресурсов за ход")]
    public ResourceChangeData[] resourceIncome;
    public ResourceChangeData[] resourceConsumption; 

    [System.Serializable]
    public class ResourceChangeData
    {
        public ResourceAmount resourceAmount;
        public string resourceDescription;
    }
}
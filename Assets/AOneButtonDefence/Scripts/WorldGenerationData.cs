using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenerationData", menuName = "ScriptableObjects/new WorldGenerationData")]
public class WorldGenerationData : ScriptableObject
{
    [field : SerializeField] public int GridSize { get; private set; } = 100;
    [field : SerializeField] public float CellSize { get; private set; } = 3.2f;
    [field: SerializeField] public int startButtonsAmount { get; private set; } = 4;
    [field : SerializeField] public BuildingsData Buildings { get; private set; }
    [field: SerializeField] public Ground CentralBlock { get; private set; }
    [field: SerializeField] public Ground EmptyBlock { get; private set; }
    [field : SerializeField] public List<Ground> EarthBlocks { get; private set; } = new List<Ground>();

    public float CellsInterval => CellSize / 2;
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameBasicData", menuName = "ScriptableObjects/new GameBasicData")]
public class GameData : ScriptableObject
{
    [field : SerializeField] public int GridSize { get; private set; } = 100;
    [field : SerializeField] public float CellSize { get; private set; } = 3.2f;
    [field : SerializeField] public UpgradeBuildings Buildings { get; private set; }
    [field: SerializeField] public Ground CentralBlock { get; private set; }
    [field: SerializeField] public Ground EmptyBlock { get; private set; }
    [field : SerializeField] public List<Ground> EarthBlocks { get; private set; } = new List<Ground>();

    public float CellsInterval => CellSize / 2;
}
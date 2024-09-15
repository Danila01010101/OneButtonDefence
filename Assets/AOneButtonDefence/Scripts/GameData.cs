using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameBasicData", menuName = "ScriptableObjects/new GameBasicData")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public int GridSize { get; private set; } = 100;
    [field: SerializeField] public float CellSize { get; private set; } = 3.2f;
    [field: SerializeField] public List<GameObject> EarthBlocks { get; private set; } = new List<GameObject>();
    [field: SerializeField] public List<GameObject> BuildingPrefabs { get; private set; } = new List<GameObject>();

    public float CellsInterval => CellSize / 2;
}
using UnityEngine;

[CreateAssetMenu(fileName = "GameBasicData", menuName = "ScriptableObjects/new GameBasicData")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public int GridSize { get; private set; } = 100;
    [field: SerializeField] public float CellSize { get; private set; } = 3.2f;

    public float CellsInterval => CellSize / 2;
}
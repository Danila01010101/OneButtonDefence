using UnityEngine;

[CreateAssetMenu(fileName = "GameBasicData", menuName = "ScriptableObjects/new GameBasicData")]
public class GameData : ScriptableObject
{
    [SerializeField] public int GrizSize { get; private set; } = 100;
    [SerializeField] public float CellSize { get; private set; } = 3.2f;

    public float CellsInterval => CellSize / 2;
}
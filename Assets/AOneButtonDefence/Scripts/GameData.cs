using UnityEngine;

[CreateAssetMenu(fileName = "GameTurnsData", menuName = "ScriptableObjects/new GameData")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public DialogueSystem StartDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem EndTurnDialogCanvas { get; private set; }
    [field: SerializeField] public int TurnAmountBeforeEvent { get; private set; } = 4;
    [field: SerializeField] public BattleWavesParameters BattleWavesParameters { get; private set; }
    [field: SerializeField] public float EnemiesSpawnSpread { get; private set; }
}
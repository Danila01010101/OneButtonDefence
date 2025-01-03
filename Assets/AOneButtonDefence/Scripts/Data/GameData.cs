using UnityEngine;

[CreateAssetMenu(fileName = "GameTurnsData", menuName = "ScriptableObjects/new GameData")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public DialogueSystem StartDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem EndTurnWinDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem EndTurnLoseDialogCanvas { get; private set; }
    [field: SerializeField] public int TurnAmountBeforeEvent { get; private set; } = 4;
    [field: SerializeField] public BattleWavesParameters BattleWavesParameters { get; private set; }
    [field: SerializeField] public float EnemiesSpawnSpread { get; private set; }
    [field: SerializeField] public Vector3 EnemiesSpawnOffset { get; private set; }
    [field: SerializeField] public float UpgradeStateDuration { get; private set; }
    [field: SerializeField] public string EnemyTag { get; private set; }
    [field: SerializeField] public string GnomeTag { get; private set; }
    [field: SerializeField] public float SwipeDeadZone { get; private set; }
}
using UnityEngine;

[CreateAssetMenu(fileName = "GameTurnsData", menuName = "ScriptableObjects/new GameData")]
public class GameData : ScriptableObject
{
    [field: Header("Start resources")]
    [field: SerializeField] public int StartFoodAmount { get; private set; }
    [field: SerializeField] public int StartMaterialsAmount { get; private set; }
    [field: SerializeField] public int StartSpiritAmount { get; private set; }
    [field: Header("Dialogs prefabs")]
    [field: SerializeField] public DialogueSystem StartDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem EndTurnWinDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem BattleLoseDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem ResourceLoseDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem SpiritLoseDialogCanvas { get; private set; }
    
    //[field: SerializeField] public int TurnAmountBeforeEvent { get; private set; } = 4;
    [field: Header("Battle parameters")]
    [field: SerializeField] public BattleWavesParameters BattleWavesParameters { get; private set; }
    [field: SerializeField] public float EnemiesSpawnSpread { get; private set; }
    [field: SerializeField] public Vector3 EnemiesSpawnOffset { get; private set; }
    [field: SerializeField] public float UpgradeStateDuration { get; private set; }
    
    [field: Header("Tags")]
    [field: SerializeField] public string EnemyTag { get; private set; }
    [field: SerializeField] public string GnomeTag { get; private set; }
    [field: Header("Input parameters")]
    [field: SerializeField] public float SwipeDeadZone { get; private set; }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTurnsData", menuName = "ScriptableObjects/new GameData")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public Vector3 WorldSize { get; private set; }

    [field: Header("Start resources")]
    [field: SerializeField] public List<StartResourceAmount> StartResources { get; private set; }
    [field: SerializeField] public ResourceData GemsResource { get; private set; }
    [field: SerializeField] public StartResourceAmount GnomeDeathSpiritFine { get; private set; }
    [field: SerializeField] public StartResourceAmount GnomeDeathWarriorFine { get; private set; }
    [field: Header("Enemy reward")]
    [field: SerializeField] public Gem EnemyRewardPrefab { get; private set; }
    [field: Header("Dialogs prefabs")]
    [field: SerializeField] public DialogueSystem StartDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem EndTurnWinDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem BattleLoseDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem FoodLoseDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem ResourceLoseDialogCanvas { get; private set; }
    [field: SerializeField] public DialogueSystem SpiritLoseDialogCanvas { get; private set; }
    
    //[field: SerializeField] public int TurnAmountBeforeEvent { get; private set; } = 4;
    [field: Header("Battle parameters")]
    [field: SerializeField] public GnomeFightingUnit GnomeUnit { get; private set; }
    [field: SerializeField] public Vector3 GnomeSpawnOffset { get; private set; }
    [field: SerializeField] public BattleWavesParameters BattleWavesParameters { get; private set; }
    [field: SerializeField] public float EnemiesSpawnSpread { get; private set; }
    [field: SerializeField] public float DefaultStoppingDistance { get; private set; } = 1;
    [field: SerializeField] public Vector3 EnemiesSpawnOffset { get; private set; }
    [field: SerializeField] public float UpgradeStateDuration { get; private set; } = 4;
    [field: SerializeField] public float UpgradeStateCompletionDelay { get; private set; } = 2;
    
    [field: Header("Tags and layers")]
    [field: SerializeField] public string EnemyLayerName { get; private set; }
    [field: SerializeField] public string GnomeLayerName { get; private set; }
    [field: SerializeField] public string EnemyTag { get; private set; }
    [field: SerializeField] public string GnomeTag { get; private set; }
    [field: Header("Input parameters")]
    [field: SerializeField] public float SwipeDeadZone { get; private set; }

    [field: SerializeField] public float ClickMaxTime { get; private set; } = 0.5f;
}
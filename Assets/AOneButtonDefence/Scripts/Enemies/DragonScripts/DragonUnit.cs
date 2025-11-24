using AOneButtonDefence.Scripts.Enemies.DragonScripts;
using UnityEngine;

public class DragonUnit : UnitWithGems
{
    [SerializeField] private UnitWithUltimateStats ultimateStats;
    [SerializeField] private DragonAnimation ultimateAnimation;
    
    protected override void InitializeStateMachine(IEnemyDetector detector)
    {
        var data = new UnitWithUltimateStateMachine.UnitWithUltimateStateMachineData(
            transform, ultimateStats, statsCounter, ultimateAnimation, navMeshComponent,
            walkingAnimation, fightAnimation, detector, this);
        stateMachine = new UnitWithUltimateStateMachine(data);
    }
}
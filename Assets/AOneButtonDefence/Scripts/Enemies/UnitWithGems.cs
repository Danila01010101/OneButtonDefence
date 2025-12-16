using System;
using UnityEngine;
using WrightAngle.Waypoint;

public class UnitWithGems : FightingUnit
{
    [SerializeField] private WaypointTarget waypoint;
    
    public static Action<Vector3, int> OnEnemyDeath;
    
    private void Start()
    {
        if (waypoint == null)
            throw new NullReferenceException();
        
        AdsReviver.RewardGranted += AdsReviverOnRewardGranted;
        waypoint.ActivateWaypoint();
        WaypointUIManager.OnWaypointTargetAdd.Invoke(waypoint);
    }

    protected void AdsReviverOnRewardGranted() => Die();

    protected override void InitializeStateMachine(IEnemyDetector detector)
    {
        var data = new WarriorStateMachine.WarriorStateMachineData(
            transform, statsCounter, characterStats.ChaseRange, characterStats.EnemyLayerMask, navMeshComponent,
            walkingAnimation, fightAnimation, detector, this, characterStats.DetectionRadius);
        stateMachine = new EnemyStateMachine(data);
    }
    
    protected override void Die()
    {
        AdsReviver.RewardGranted -= AdsReviverOnRewardGranted;
        int gemsAmount = UnityEngine.Random.Range(characterStats.MinGemSpawn, characterStats.MaxGemSpawn);
        EnemyDeathManager.Instance.NotifyEnemyDeath(transform.position, gemsAmount);
        waypoint.DestroyWaypoint();
        base.Die();
    }
}
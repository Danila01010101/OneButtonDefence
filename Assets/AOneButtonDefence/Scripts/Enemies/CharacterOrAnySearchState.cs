using UnityEngine;

public class CharacterOrAnySearchState : TargetSearchState
{
    private readonly Transform preferredTarget;
    private readonly IStateChanger stateMachine;
    private readonly ITargetFollower targetFollower;
    private readonly Transform transform;
    private readonly IEnemyDetector detector;

    public CharacterOrAnySearchState(TargetSearchStateData data, CharacterStatsCounter statsCounter) 
        : base(data, statsCounter)
    {
        var character = Object.FindObjectOfType<CharacterController>();
        
        if (character != null)
        {
            preferredTarget = character.transform;
        }
        
        stateMachine = data.StateMachine;
        targetFollower = data.TargetFollower;
        transform = data.SelfTransform;
        detector = data.Detector;
    }

    protected override void LookForTarget()
    {
        Transform detectedEnemy = null;
        float stoppingDistance = 0f;

        if (preferredTarget != null && preferredTarget.TryGetComponent<IDamagable>(out var damagable) && damagable.IsAlive())
        {
            detectedEnemy = preferredTarget;
            stoppingDistance = 1.5f;
        }
        else if (transform != null)
        {
            var enemyInfo = detector.GetClosestEnemy(transform.position, detectionRadius);
            
            if (enemyInfo.Target != null)
            {
                detectedEnemy = enemyInfo.Target;
                stoppingDistance = enemyInfo.TargetRadius;
            }
        }

        if (detectedEnemy == null)
            return;

        targetFollower.SetTarget(detectedEnemy, stoppingDistance);
        stateMachine.ChangeState<TargetFollowingState>();
    }
}
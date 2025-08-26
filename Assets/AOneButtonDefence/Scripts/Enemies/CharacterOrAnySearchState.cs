using UnityEngine;

public class CharacterOrAnySearchState : TargetSearchState
{
    private readonly Transform preferredTarget;
    private readonly IStateChanger stateMachine;
    private readonly ITargetFollower targetFollower;
    private readonly Transform transform;
    private readonly IEnemyDetector detector;

    public CharacterOrAnySearchState(TargetSearchStateData data) 
        : base(data)
    {
        var character = Object.FindObjectOfType<CharacterController>();
        
        if (character != null)
        {
            preferredTarget = character.transform;
        }
        
        this.stateMachine = data.StateMachine;
        this.targetFollower = data.TargetFollower;
        this.transform = data.Transform;
        this.detector = data.Detector;
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
            var enemyInfo = detector.GetClosestEnemy(transform.position);
            
            if (enemyInfo.Target != null)
            {
                detectedEnemy = enemyInfo.Target;
                stoppingDistance = enemyInfo.TargetRadius;
            }
        }

        if (detectedEnemy == null)
            return;

        Debug.Log("Found enemy, switching to " + detectedEnemy.gameObject.name);
        targetFollower.SetTarget(detectedEnemy, stoppingDistance);
        stateMachine.ChangeState<TargetFollowingState>();
    }
}
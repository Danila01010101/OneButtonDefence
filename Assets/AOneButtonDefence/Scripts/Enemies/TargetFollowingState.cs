using UnityEngine;
using UnityEngine.AI;

public class TargetFollowingState : IState, ITargetFollower
{
    private Transform target;
    private IEnemyDetector detector;
    private float chaseRange;
    
    private readonly IStateChanger stateMachine;
    private readonly ITargetAttacker targetAttacker;
    private readonly NavMeshAgent agent;
    private readonly Transform transform;
    private readonly LayerMask targetMask;
    private readonly WalkingAnimation animation;
    private readonly float defaultChaseRange;
    private readonly ISelfDamageable selfDamageable;

    public TargetFollowingState(
        IStateChanger stateMachine,
        NavMeshAgent agent,
        CharacterStats stats,
        ITargetAttacker targetAttacker,
        LayerMask targetMask,
        WalkingAnimation animation,
        IEnemyDetector detector,
        ISelfDamageable selfDamageable)
    {
        this.stateMachine = stateMachine;
        this.agent = agent;
        defaultChaseRange = stats.ChaseRange;
        transform = agent.transform;
        this.targetAttacker = targetAttacker;
        this.targetMask = targetMask;
        this.animation = animation;
        this.detector = detector;
        this.selfDamageable = selfDamageable;
    }

    public void Enter()
    {
        animation.StartAnimation();
        detector.NewEnemiesDetected += CheckIfTargetChanged;
        selfDamageable.DamageRecieved += OnDamageReceived;
    }

    public void Exit()
    {
        animation.StopAnimation();
        target = null;
        detector.NewEnemiesDetected -= CheckIfTargetChanged;
        selfDamageable.DamageRecieved -= OnDamageReceived;
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate()
    {
        if (IsTargetGone())
        {
            stateMachine.ChangeState<TargetSearchState>();
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, target.position);

        if (distanceToEnemy < chaseRange)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, targetMask);
            IDamagable foundTarget = FindTarget(colliders);

            if (foundTarget != null)
            {
                targetAttacker.SetTarget(foundTarget);
                stateMachine.ChangeState<FightState>();
            }
        }
    }

    public void Update()
    {
        if (IsTargetGone())
        {
            stateMachine.ChangeState<TargetSearchState>();
            return;
        }

        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform transform) => target = transform;

    private bool IsTargetGone() => target == null;

    private void CheckIfTargetChanged() => stateMachine.ChangeState<TargetSearchState>();

    private IDamagable FindTarget(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            IDamagable foundTarget;

            if (collider.transform != transform && collider.gameObject.TryGetComponent(out foundTarget))
            {
                return foundTarget;
            }
        }

        return null;
    }

    public void SetTarget(Transform transform, float enemyStoppingDistance)
    {
        target = transform;
        chaseRange = enemyStoppingDistance > defaultChaseRange ? enemyStoppingDistance : defaultChaseRange;
    }

    private void OnDamageReceived(IDamagable attacker)
    {
        if (attacker != null && attacker.GetTransform().gameObject.name != "BaseMageCircle")
        {
            SetTarget(attacker.GetTransform());
        }
    }
}
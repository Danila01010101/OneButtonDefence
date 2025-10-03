using UnityEngine;
using UnityEngine.AI;

public class TargetFollowingState : UnitStateBase, ITargetFollower
{
    private Transform target;
    private readonly ITargetAttacker targetAttacker;
    private readonly LayerMask targetMask;
    private readonly WalkingAnimation animation;
    private readonly IEnemyDetector detector;
    private readonly ISelfDamageable selfDamageable;
    private readonly NavMeshAgent agent;

    private readonly float defaultChaseRange;
    private float chaseRange;

    protected readonly bool IsPlayerControlled;
    protected readonly float DefaultSpeed;

    protected float UnitSpeedWithBuff => DefaultSpeed * Mathf.Pow(1.01f, GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.WarriorSpeed));

    public TargetFollowingState(
        IStateChanger stateMachine,
        NavMeshAgent agent,
        CharacterStats stats,
        ITargetAttacker targetAttacker,
        LayerMask targetMask,
        WalkingAnimation animation,
        IEnemyDetector detector,
        ISelfDamageable selfDamageable,
        bool isPlayerControlled)
        : base(stateMachine, agent.transform, isPlayerControlled)
    {
        this.targetAttacker = targetAttacker;
        this.targetMask = targetMask;
        this.animation = animation;
        this.detector = detector;
        this.agent = agent;
        this.selfDamageable = selfDamageable;
        this.defaultChaseRange = stats.ChaseRange;
        this.DefaultSpeed = stats.Speed;
        this.IsPlayerControlled = isPlayerControlled;
    }

    public override void Enter()
    {
        animation.StartAnimation();
        detector.NewEnemiesDetected += CheckIfTargetChanged;
        selfDamageable.DamageRecieved += OnDamageReceived;
        agent.speed = IsPlayerControlled ? UnitSpeedWithBuff : DefaultSpeed;
    }

    public override void Exit()
    {
        animation.StopAnimation();
        target = null;
        detector.NewEnemiesDetected -= CheckIfTargetChanged;
        selfDamageable.DamageRecieved -= OnDamageReceived;
    }

    public override void PhysicsUpdate()
    {
        if (IsTargetGone())
        {
            StateMachine.ChangeState<TargetSearchState>();
            return;
        }

        float distanceToEnemy = Vector3.Distance(SelfTransform.position, target.position);

        if (distanceToEnemy < chaseRange)
        {
            Collider[] colliders = Physics.OverlapSphere(SelfTransform.position, chaseRange, targetMask);
            IDamagable foundTarget = FindTarget(colliders);

            if (foundTarget != null)
            {
                targetAttacker.SetTarget(foundTarget);
                StateMachine.ChangeState<FightState>();
            }
        }
    }

    public override void Update()
    {
        if (IsTargetGone())
        {
            StateMachine.ChangeState<TargetSearchState>();
            return;
        }

        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform transform) => target = transform;

    public void SetTarget(Transform transform, float enemyStoppingDistance)
    {
        target = transform;
        chaseRange = enemyStoppingDistance > defaultChaseRange ? enemyStoppingDistance : defaultChaseRange;
    }

    private bool IsTargetGone() => target == null;

    private void CheckIfTargetChanged() => StateMachine.ChangeState<TargetSearchState>();

    private IDamagable FindTarget(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.transform != SelfTransform && collider.TryGetComponent(out IDamagable foundTarget))
            {
                return foundTarget;
            }
        }
        return null;
    }

    private void OnDamageReceived(IDamagable attacker)
    {
        if (attacker != null && attacker.GetTransform().gameObject.layer != LayerMask.NameToLayer("Spell"))
        {
            SetTarget(attacker.GetTransform());
        }
    }
}
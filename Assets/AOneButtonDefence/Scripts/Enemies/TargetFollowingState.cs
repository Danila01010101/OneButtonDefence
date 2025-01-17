using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TargetFollowingState : IState, ITargetFollower
{
    private IStateChanger stateMachine;
    private ITargetAttacker targetAttacker;
    private NavMeshAgent agent;
    private Transform transform;
    private Transform target;
    private LayerMask targetMask;
    private WalkingAnimation animation;
    private float speed;
    private float attackRange;
    private bool IsTargetGone() => target == null;

    public TargetFollowingState(IStateChanger stateMachine, NavMeshAgent agent, CharacterStats stats, 
        ITargetAttacker targetAttacker, LayerMask targetMask, WalkingAnimation animation)
    {
        this.stateMachine = stateMachine;
        this.agent = agent;
        speed = stats.Speed;
        attackRange = stats.AttackRange;
        transform = agent.transform;
        this.targetAttacker = targetAttacker;
        this.targetMask = targetMask;
        this.animation = animation;
    }

    public void Enter()
    {
        animation.StartAnimation();
    }

    public void Exit()
    {
        animation.StopAnimation();
        target = null;
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

        if (distanceToEnemy < attackRange)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetMask);
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

    private Vector3 GetDirection() => (target.position - transform.position).normalized * speed;

    private IDamagable FindTarget(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            IDamagable foundTarget;

            if (collider.transform != transform && collider.gameObject.TryGetComponent<IDamagable>(out foundTarget))
            {
                return foundTarget;
            }
        }

        return null;
    }
}
using UnityEngine;

public class TargetFollowingState : IState, ITargetFollower
{
    private IStateChanger stateMachine;
    private ITargetAttacker targetAttacker;
    private CharacterController characterController;
    private Transform transform;
    private Transform target;
    private float speed;
    private float attackRange;
    private bool IsTargetExists() => target == null;

    public TargetFollowingState(IStateChanger stateMachine, CharacterController characterController, CharacterStats stats, ITargetAttacker targetAttacker)
    {
        this.stateMachine = stateMachine;
        this.characterController = characterController;
        this.speed = stats.Speed;
        this.attackRange = stats.AttackRange;
        transform = characterController.transform;
        this.targetAttacker = targetAttacker;
    }

    public void Enter()
    {

    }

    public void Exit()
    {
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
        if (IsTargetExists())
        {
            stateMachine.ChangeState<TargetSearchState>();
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, target.position);

        if (distanceToEnemy < attackRange)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
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
        characterController.Move(GetDirection());
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
using System.Collections;
using UnityEngine;

public class FightState : IState, ITargetAttacker
{
    private IStateChanger stateMachine;
    private IDamagable target;
    private MonoBehaviour coroutineStarter;
    private FightAnimation animation;
    private bool isTargetSetted;
    private float lastTimeAttacked;
    private float attackDelay;
    private int damage;

    public FightState(IStateChanger stateChanger, float attackDelay, int damage, FightAnimation animation)
    {
        stateMachine = stateChanger;
        this.attackDelay = attackDelay;
        this.damage = damage;
        this.animation = animation;
    }

    public void SetTarget(IDamagable target)
    {
        isTargetSetted = true;
        this.target = target;
    }

    public void Enter()
    {
        animation.CharacterAttacked += Attack;
    }

    public void Exit() 
    {
        target = null;
        isTargetSetted = false;
        animation.CharacterAttacked -= Attack;
        animation.InterruptAnimation();
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update()
    {
        if (lastTimeAttacked + attackDelay >= Time.time)
            return;

        CheckTarget();
        lastTimeAttacked = Time.time;
        animation.StartAnimation();
    }

    private void Attack()
    {
        if (isTargetSetted)
            target.TakeDamage(damage);
    }

    private void CheckTarget()
    {
        if (target == null || !target.IsAlive())
        {
            stateMachine.ChangeState<TargetSearchState>();
        }
    }
}
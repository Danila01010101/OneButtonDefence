using System.Collections;
using UnityEngine;

public class FightState : IState, ITargetAttacker
{
    private IStateChanger stateMachine;
    private IDamagable target;
    private MonoBehaviour coroutineStarter;
    private bool isTargetSetted = false;
    private float lastTimeAttacked;
    private float attackDelay;
    private int damage;

    public FightState(IStateChanger stateChanger, float attackDelay, int damage)
    {
        this.stateMachine = stateChanger;
        this.attackDelay = attackDelay;
        this.damage = damage;
    }

    public void Enter()
    {
        
    }

    public void Exit() 
    {
        target = null;
        isTargetSetted = false;
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
        if (!isTargetSetted)
            return;

        if (target == null || !target.IsAlive())
        {
            stateMachine.ChangeState<TargetSearchState>();
            return;
        }

        if (lastTimeAttacked + attackDelay >= Time.time)
            return;

        target.TakeDamage(damage);
        lastTimeAttacked = Time.time;
    }

    public void SetTarget(IDamagable target)
    {
        isTargetSetted = true;
        this.target = target;
    }
}
using System.Collections;
using UnityEngine;

public class FightState : IState, ITargetAttacker
{
    private IStateChanger stateMachine;
    private IDamagable target;
    private MonoBehaviour coroutineStarter;
    private float attackDelay;
    private int damage;

    public FightState(IStateChanger stateChanger, MonoBehaviour coroutineStarter, float attackDelay, int damage)
    {
        this.stateMachine = stateChanger;
        this.coroutineStarter = coroutineStarter;
        this.attackDelay = attackDelay;
        this.damage = damage;
    }

    public void Enter()
    {
        coroutineStarter.StartCoroutine(AtackProcess());
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

    public void PhysicsUpdate() { }

    public void Update() { }

    public void SetTarget(IDamagable target)
    {
        this.target = target;
    }

    private IEnumerator AtackProcess()
    {
        while (target.IsAlive())
        {
            target.TakeDamage(damage);
            yield return new WaitForSeconds(attackDelay);
        }

        stateMachine.ChangeState<TargetSearchState>();
    }
}
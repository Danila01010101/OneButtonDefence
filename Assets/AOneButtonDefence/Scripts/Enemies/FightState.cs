using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;

public class FightState : IState, ITargetAttacker
{
    protected readonly IStateChanger StateMachine;
    protected readonly IAttackAnimator Animation;
    protected readonly float AttackDelay;
    protected readonly int Damage;
    
    protected bool IsTargetSetted;
    
    private IDamagable Target;
    private float LastTimeAttacked;

    public FightState(IStateChanger stateChanger, float attackDelay, int damage, IAttackAnimator animation)
    {
        StateMachine = stateChanger;
        AttackDelay = attackDelay;
        Damage = damage;
        Animation = animation;
    }

    public void SetTarget(IDamagable target)
    {
        IsTargetSetted = true;
        this.Target = target;
    }

    public virtual void Enter()
    {
        Animation.CharacterAttacked += Attack;
    }

    public virtual void Exit() 
    {
        Target = null;
        IsTargetSetted = false;
        Animation.CharacterAttacked -= Attack; 
        Animation.InterruptAnimation();
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public virtual void Update()
    {
        if (LastTimeAttacked + AttackDelay >= Time.time)
            return;

        CheckTarget();
        LastTimeAttacked = Time.time;
        Animation.StartAnimation();
    }

    protected virtual void Attack()
    {
        if (IsTargetSetted)
            Target.TakeDamage(Damage);
    }

    private void CheckTarget()
    {
        if (Target == null || Target.IsAlive() == false)
        {
            StateMachine.ChangeState<TargetSearchState>();
        }
    }
}
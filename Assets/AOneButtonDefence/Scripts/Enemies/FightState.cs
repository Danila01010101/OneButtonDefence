using AOneButtonDefence.Scripts.Interfaces;
using System.Data.Common;
using UnityEngine;

public class FightState : IState, ITargetAttacker
{
    protected readonly IStateChanger StateMachine;
    protected readonly IAttackAnimator Animation;
    protected readonly ISelfDamageable SelfDamageable;
    protected readonly float AttackDelay;
    protected readonly int BasicDamage;
    protected readonly int DamageUpgradeValue;
    protected readonly float DefaultDistanceToLoseTarget = 1.2f;

    protected int Damage => BasicDamage + DamageUpgradeValue * GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.StrengthBuff);
    protected bool IsTargetSetted;
    
    private IDamagable Target;
    private float LastTimeAttacked;

    public FightState(IStateChanger stateChanger, float attackDelay, int damage, int damageUpgradeValue, IAttackAnimator animation, ISelfDamageable selfDamageable)
    {
        StateMachine = stateChanger;
        AttackDelay = attackDelay;
        BasicDamage = damage;
        DamageUpgradeValue = damageUpgradeValue;
        Animation = animation;
        SelfDamageable = selfDamageable;
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
        Debug.Log(string.Format("Damage dealed is {0}, buff damage is {1}. Target name is ", Damage,
            GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.StrengthBuff) * DamageUpgradeValue) + Target.GetName());

        if (IsTargetSetted)
            Target.TakeDamage(SelfDamageable.GetSelfDamagable(), Damage);
    }

    private void CheckTarget()
    {
        if (Target == null || Target.IsAlive() == false || 
            Vector3.Distance(Target.GetTransform().position, SelfDamageable.GetSelfDamagable().GetTransform().position) > DefaultDistanceToLoseTarget)
        {
            StateMachine.ChangeState<TargetSearchState>();
        }
    }
}
using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;

public class FightState : UnitStateBase, ITargetAttacker
{
    protected readonly IAttackAnimator Animation;
    protected readonly ISelfDamageable SelfDamageable;
    protected readonly float AttackDelay;
    protected readonly int BasicDamage;
    protected readonly int DamageUpgradeValue;
    protected readonly float DefaultDistanceToLoseTarget = 1.2f;

    protected float CalculatedAttackDelay { get; private set; }
    protected float AttackDelayWithBuff => AttackDelay * Mathf.Pow(0.99f, GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.WarriorSpeed));
    protected int Damage => BasicDamage + DamageUpgradeValue * GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.StrengthBuff);
    protected bool IsTargetSetted;

    private IDamagable Target;
    private float LastTimeAttacked;

    public FightState(
        IStateChanger stateChanger,
        float attackDelay,
        int damage,
        int damageUpgradeValue,
        IAttackAnimator animation,
        ISelfDamageable selfDamageable,
        bool isPlayerControlled)
        : base(stateChanger, selfDamageable.GetSelfDamagable().GetTransform(), isPlayerControlled)
    {
        AttackDelay = attackDelay;
        BasicDamage = damage;
        DamageUpgradeValue = damageUpgradeValue;
        Animation = animation;
        SelfDamageable = selfDamageable;
    }

    public void SetTarget(IDamagable target)
    {
        IsTargetSetted = true;
        Target = target;
    }

    public override void Enter()
    {
        Animation.CharacterAttacked += Attack;
        CalculatedAttackDelay = IsPlayerControlled ? AttackDelayWithBuff : AttackDelay;
    }

    public override void Exit()
    {
        Target = null;
        IsTargetSetted = false;
        Animation.CharacterAttacked -= Attack;
        Animation.InterruptAnimation();
    }

    public override void Update()
    {
        if (LastTimeAttacked + CalculatedAttackDelay >= Time.time)
            return;

        CheckTarget();
        LastTimeAttacked = Time.time;
        Animation.StartAnimation();
    }

    protected virtual void Attack()
    {
        if (Target == null) return;
        Target.TakeDamage(SelfDamageable.GetSelfDamagable(), Damage);
    }

    private void CheckTarget()
    {
        var selfTransform = SelfDamageable.GetSelfDamagable().GetTransform();
        if (Target == null || Target.IsAlive() == false ||
            Vector3.Distance(Target.GetTransform().position, selfTransform.position) > DefaultDistanceToLoseTarget)
        {
            StateMachine.ChangeState<TargetSearchState>();
        }
    }
}
using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;

public class FightState : UnitStateBase, ITargetAttacker
{
    protected readonly IAttackAnimator Animation;
    protected readonly ISelfDamageable SelfDamageable;
    protected readonly CharacterStatsCounter CharacterStatsCounter;
    protected readonly float DefaultDistanceToLoseTarget = 1.2f;

    protected float AttackDelay => CharacterStatsCounter.GetStat(ResourceData.ResourceType.WarriorAttackSpeed);
    protected float Damage => CharacterStatsCounter.GetStat(ResourceData.ResourceType.StrengthBuff);
    protected bool IsTargetSetted;

    private IDamagable Target;
    private float LastTimeAttacked;

    public FightState(
        IStateChanger stateChanger,
        CharacterStatsCounter statsCounter,
        IAttackAnimator animation,
        ISelfDamageable selfDamageable,
        bool isPlayerControlled)
        : base(stateChanger, selfDamageable.GetSelfDamagable().GetTransform(), statsCounter, isPlayerControlled)
    {
        CharacterStatsCounter = statsCounter;
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
        if (LastTimeAttacked + AttackDelay >= Time.time)
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
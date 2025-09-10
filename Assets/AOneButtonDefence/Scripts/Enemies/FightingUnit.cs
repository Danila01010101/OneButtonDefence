    using System;
    using UnityEngine;
    using UnityEngine.AI;
    using DG.Tweening;

[RequireComponent(typeof(WalkingAnimation))]
[RequireComponent(typeof(FightAnimation))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(DeathAnimation))]
public class FightingUnit : MonoBehaviour, IDamagable, ISelfDamageable
{
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected Renderer render;

    private Health health;
    
    protected NavMeshAgent navMeshComponent;
    protected IStateChanger stateMachine;
    protected FightAnimation fightAnimation;
    protected WalkingAnimation walkingAnimation;
    protected DeathAnimation deathAnimation;
    protected MaterialChanger materialChanger;
    protected AudioSource audioSource;

    public event Action<IDamagable> DamageRecieved;

    public virtual void Initialize(IEnemyDetector detector)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = characterStats.DeathSound;
        InitializeAnimationComponents();
        navMeshComponent = GetComponent<NavMeshAgent>();
        health = new Health(characterStats.Health);
        health.Death += Die;
        InitializeStateMachine(detector);
        materialChanger = new MaterialChanger(this);
        materialChanger.ChangeMaterialColour(render, characterStats.StartColor, characterStats.EndColor, characterStats.FadeDuration,characterStats.Delay);
        AudioSettings.AddAudioSource(audioSource);
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    protected virtual void FixedUpdate() 
    { 
        stateMachine.PhysicsUpdate();
    }

    public virtual void TakeDamage(IDamagable damagerTransform, int damage)
    {
        health.TakeDamage(damagerTransform.GetTransform(), damage);
        DamageRecieved?.Invoke(damagerTransform);
    }

    public IDamagable GetSelfDamagable() => this;

    public Transform GetTransform() => transform;
    
    public string GetName()
    {
        return gameObject.name;
    }

    public bool IsAlive() => health.Amount > 0;

    protected virtual void InitializeAnimationComponents()
    {
        fightAnimation = GetComponent<FightAnimation>();
        walkingAnimation = GetComponent<WalkingAnimation>();
        deathAnimation = GetComponent<DeathAnimation>();
    }

    protected virtual void InitializeStateMachine(IEnemyDetector detector)
    {
        var data = new WarriorStateMachine.WarriorStateMachineData(
            transform, characterStats, navMeshComponent,
            walkingAnimation, fightAnimation, detector, this);
        stateMachine = new WarriorStateMachine(data);
    }

    protected virtual void Die()
    {
        deathAnimation.StartAnimation();
        materialChanger.ChangeMaterialColour(render, characterStats.EndColor, characterStats.StartColor, audioSource.clip.length,characterStats.Delay);
        stateMachine.Exit();
        Destroy(gameObject, audioSource.clip.length + 0.005f);
    }
}
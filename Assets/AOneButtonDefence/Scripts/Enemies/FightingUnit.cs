using System;
using System.Collections.Generic;
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
    
    protected AudioClip currentDeathSound;
    protected NavMeshAgent navMeshComponent;
    protected IStateChanger stateMachine;
    protected FightAnimation fightAnimation;
    protected WalkingAnimation walkingAnimation;
    protected DeathAnimation deathAnimation;
    protected MaterialChanger materialChanger;
    protected AudioSource audioSource;
    protected CharacterStatsCounter statsCounter;

    private bool isDead;

    public event Action<IDamagable> DamageRecieved;

    public virtual void Initialize(IEnemyDetector detector)
    {
        audioSource = GetComponent<AudioSource>();
        InitializeAnimationComponents();
        navMeshComponent = GetComponent<NavMeshAgent>();
        InitializeStats();
        health.Death += Die;
        InitializeStateMachine(detector);
        materialChanger = new MaterialChanger(this);
        materialChanger.ChangeMaterialColour(render, characterStats.StartColor, characterStats.EndColor, characterStats.FadeDuration, characterStats.Delay);
        AudioSettings.AddAudioSource(audioSource);
        currentDeathSound = characterStats.DeathSound;
    }

    protected void InitializeStats()
    {
        statsCounter = new CharacterStatsCounter();
        statsCounter.AddStat(ResourceData.ResourceType.WarriorHealth, new Health(characterStats.Health));
        health = statsCounter.GetStat<Health>(ResourceData.ResourceType.WarriorHealth);
        statsCounter.AddStat(ResourceData.ResourceType.StrengthBuff, new DefaultStat(characterStats.Damage));
        statsCounter.AddStat(ResourceData.ResourceType.WarriorSpeed, new DefaultStat(characterStats.Speed));
        statsCounter.AddStat(ResourceData.ResourceType.WarriorAttackSpeed, new DefaultStat(characterStats.AttackDelay));
    }

    protected virtual void Update()
    {
        if (isDead || stateMachine == null)
            return;
        
        stateMachine?.Update();
    }

    protected virtual void FixedUpdate() 
    {
        if (isDead) return;
        stateMachine?.PhysicsUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        stateMachine?.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (isDead) return;
        stateMachine?.OnTriggerExit(other);
    }

    public virtual void TakeDamage(IDamagable damagerTransform, float damage)
    {
        if (isDead) return;
        health.TakeDamage(damagerTransform.GetTransform(), damage);
        DamageRecieved?.Invoke(damagerTransform);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        health.TakeDamage(damage);
    }

    public IDamagable GetSelfDamagable() => this;

    public Transform GetTransform() => transform;
    
    public string GetName() => gameObject.name;

    public bool IsAlive() => health.Value > 0;

    protected virtual void InitializeAnimationComponents()
    {
        fightAnimation = GetComponent<FightAnimation>();
        walkingAnimation = GetComponent<WalkingAnimation>();
        deathAnimation = GetComponent<DeathAnimation>();
    }

    protected virtual void InitializeStateMachine(IEnemyDetector detector)
    {
        var data = new WarriorStateMachine.WarriorStateMachineData(
            transform, statsCounter, characterStats.ChaseRange, characterStats.EnemyLayerMask, navMeshComponent,
            walkingAnimation, fightAnimation, detector, this, characterStats.DetectionRadius);
        stateMachine = new WarriorStateMachine(data);
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        if (health != null)
            health.Death -= Die;

        deathAnimation.StartAnimation();
        materialChanger.ChangeMaterialColour(render, characterStats.EndColor, characterStats.StartColor, 0.5f, characterStats.Delay);
        stateMachine?.Exit();
        stateMachine = null;

        if (audioSource != null && audioSource.clip != null)
        {
            AudioUtility.PlayClipAtPoint(audioSource, transform.position);
        }

        Destroy(gameObject, 0.1f);
    }
}
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

    private bool isDead;

    public event Action<IDamagable> DamageRecieved;

    public virtual void Initialize(IEnemyDetector detector)
    {
        audioSource = GetComponent<AudioSource>();
        InitializeAnimationComponents();
        navMeshComponent = GetComponent<NavMeshAgent>();
        health = new Health(characterStats.Health);
        health.Death += Die;
        InitializeStateMachine(detector);
        materialChanger = new MaterialChanger(this);
        materialChanger.ChangeMaterialColour(render, characterStats.StartColor, characterStats.EndColor, characterStats.FadeDuration, characterStats.Delay);
        AudioSettings.AddAudioSource(audioSource);
    }

    protected virtual void Update()
    {
        if (isDead) return;
        stateMachine.Update();
    }

    protected virtual void FixedUpdate() 
    {
        if (isDead) return;
        stateMachine.PhysicsUpdate();
    }

    public virtual void TakeDamage(IDamagable damagerTransform, int damage)
    {
        if (isDead) return;
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
        if (isDead) return;
        isDead = true;
        if (health != null)
            health.Death -= Die;
        deathAnimation.StartAnimation();
        materialChanger.ChangeMaterialColour(render, characterStats.EndColor, characterStats.StartColor, 0.5f, characterStats.Delay);
        stateMachine?.Exit();

        if (characterStats.DeathSound != null)
        {
            AudioSource.PlayClipAtPoint(characterStats.DeathSound, transform.position);
        }

        Destroy(gameObject, 0.1f);
    }
}
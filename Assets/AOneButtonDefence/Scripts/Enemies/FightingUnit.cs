    using UnityEngine;
    using UnityEngine.AI;
    using DG.Tweening;

[RequireComponent(typeof(WalkingAnimation))]
[RequireComponent(typeof(FightAnimation))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(DeathAnimation))]
public class FightingUnit : MonoBehaviour, IDamagable
{
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected Renderer render;

    private Health health;
    private NavMeshAgent navMeshComponent;
    private WarriorStateMachine stateMachine;
    private FightAnimation fightAnimation;
    private WalkingAnimation walkingAnimation;
    private DeathAnimation deathAnimation;
    private MaterialChanger materialChanger;
    private AudioSource audioSource;
    
    private IEnemyDetector detector;

    public virtual void Initialize(IEnemyDetector detector)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = characterStats.DeathSound;
        this.detector = detector;
        InitializeAnimationComponents();
        navMeshComponent = GetComponent<NavMeshAgent>();
        health = new Health(characterStats.Health);
        health.Death += Die;
        InitializeStateMachine();
        materialChanger = new MaterialChanger(this);
        materialChanger.ChangeMaterialColour(render, characterStats.StartColor, characterStats.EndColor, characterStats.FadeDuration,characterStats.Delay);
        SoundSettings.AddAudioSource(audioSource);
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    protected virtual void FixedUpdate() 
    { 
        stateMachine.PhysicsUpdate();
    }

    public virtual void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    public bool IsAlive() => health.amount > 0;

    protected virtual void InitializeAnimationComponents()
    {
        fightAnimation = GetComponent<FightAnimation>();
        walkingAnimation = GetComponent<WalkingAnimation>();
        deathAnimation = GetComponent<DeathAnimation>();
    }

    private void InitializeStateMachine()
    {
        var data = new WarriorStateMachine.WarriorStateMachineData(
            transform, characterStats, navMeshComponent,
            walkingAnimation, fightAnimation, detector);
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
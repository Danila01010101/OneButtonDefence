using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WalkingAnimation))]
[RequireComponent(typeof(FightAnimation))]
[RequireComponent(typeof(NavMeshAgent))]
public class FightingUnit : MonoBehaviour, IDamagable
{
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected Renderer render;

    private Health health;
    private NavMeshAgent navMeshComponent;
    private WarriorStateMachine stateMachine;
    private FightAnimation fightAnimation;
    private WalkingAnimation walkingAnimation;
    private MaterialChanger materialChanger;
    
    private IEnemyDetector detector;

    protected virtual void Start()
    {
        InitializeAnimationComponents();
        navMeshComponent = GetComponent<NavMeshAgent>();
        health = new Health(characterStats.Health);
        health.Death += Die;
        InitializeStateMachine();
        materialChanger = new MaterialChanger(this);
        materialChanger.ChangeMaterialColour(render, characterStats.StartColor, characterStats.EndColor, characterStats.FadeDuration,characterStats.Delay);
    }

    public virtual void Initialize(IEnemyDetector detector)
    {
        this.detector = detector;
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
        materialChanger.ChangeMaterialColour(render, characterStats.EndColor, characterStats.StartColor, 0.1f,characterStats.Delay);
        Destroy(gameObject, 0.11f);
    }
}
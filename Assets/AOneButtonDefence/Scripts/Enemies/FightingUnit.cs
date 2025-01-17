using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WalkingAnimation))]
[RequireComponent(typeof(FightAnimation))]
[RequireComponent(typeof(NavMeshAgent))]
public class FightingUnit : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Renderer render;

    private Health health;
    private NavMeshAgent navMeshComponent;
    private EnemyStateMachine stateMachine;
    private FightAnimation fightAnimation;
    private WalkingAnimation walkingAnimation;
    private MaterialChanger materialChanger;

    private void Start()
    {
        InitializeAnimationComponents();
        navMeshComponent = GetComponent<NavMeshAgent>();
        health = new Health(characterStats.Health);
        health.Death += Die;
        InitializeStateMachine();
        materialChanger = new MaterialChanger(this);
        materialChanger.ChangeMaterialColour(render, characterStats.StartColor, characterStats.EndColor, characterStats.FadeDuration,characterStats.Delay);
    }

    private void Update() => stateMachine.Update();

    private void FixedUpdate() => stateMachine.PhysicsUpdate();

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        Debug.Log(gameObject.name + "is taking damage! Health is  " + health.amount);
    }

    public bool IsAlive() => health.amount > 0;

    private void InitializeAnimationComponents()
    {
        fightAnimation = GetComponent<FightAnimation>();
        walkingAnimation = GetComponent<WalkingAnimation>();
    }

    private void InitializeStateMachine()
    {
        var data = new EnemyStateMachine.EnemyStateMachineData(
            transform, characterStats, navMeshComponent, this,
            walkingAnimation, fightAnimation);
        stateMachine = new EnemyStateMachine(data);
    }

    private void Die()
    {
        materialChanger.ChangeMaterialColour(render, characterStats.EndColor, characterStats.StartColor, 0.1f,characterStats.Delay);
        Destroy(gameObject, 0.11f);
    }
}
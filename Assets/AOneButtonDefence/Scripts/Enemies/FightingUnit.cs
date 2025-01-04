using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GoingAnimation))]
[RequireComponent(typeof(FightAnimation))]
[RequireComponent(typeof(NavMeshAgent))]
public class FightingUnit : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Renderer render;

    private Health health;
    private NavMeshAgent navMeshComponent;
    private EnemieStateMachine stateMachine;
    private FightAnimation fightAnimation;

    private void Start()
    {
        fightAnimation = GetComponent<FightAnimation>();
        navMeshComponent = GetComponent<NavMeshAgent>();
        health = new Health(characterStats.Health);
        health.Death += Die;
        navMeshComponent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemieStateMachine(transform, characterStats, navMeshComponent, this);
        StartInvisibleColor(render, characterStats.StartColor, characterStats.EndColor, characterStats.FadeDuration,characterStats.Delay);
    }

    public void TakeDamage(int damage) => health.TakeDamage(damage);

    public bool IsAlive() => health.amount > 0;

    private void Update() => stateMachine.Update();

    private void FixedUpdate() => stateMachine.PhysicsUpdate();

    private void StartInvisibleColor(Renderer renderer, Color startcolor, Color endcolor, float duration, float startfordisinvis)
    {
        Material material = renderer.material;
        material.color = startcolor;
        StartCoroutine(Fade(material, endcolor, duration, startfordisinvis));
    }

    private IEnumerator Fade(Material material, Color endcolor, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        material.DOColor(endcolor, duration);
        yield return new WaitForSeconds(duration);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
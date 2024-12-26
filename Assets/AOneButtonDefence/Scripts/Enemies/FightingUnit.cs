using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GoingAnimation))]
[RequireComponent(typeof(FightAnimation))]
[RequireComponent(typeof(CharacterController))]
public class FightingUnit : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Renderer render;

    private Health health;
    private CharacterController characterController;
    private EnemieStateMachine stateMachine;
    private FightAnimation fightAnimation;

    private void Start()
    {
        fightAnimation = GetComponent<FightAnimation>();
        health = new Health(characterStats.Health);
        health.Death += Die;
        characterController = GetComponent<CharacterController>();
        stateMachine = new EnemieStateMachine(transform, characterStats, characterController, this);
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
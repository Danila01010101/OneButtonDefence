using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasicKnight : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterStats characterStats;
    private Health health;
    private CharacterController characterController;
    private EnemieStateMachine stateMachine;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        stateMachine = new EnemieStateMachine(transform, characterStats, characterController, this);
    }

    public void TakeDamage(int damage) => health.TakeDamage(damage);

    public bool IsAlive() => health.amount > 0;

    private void Update() => stateMachine.Update();

    private void FixedUpdate() => stateMachine.PhysicsUpdate();
}
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasicKnight : MonoBehaviour, IDamagable
{
    private CharacterStats characterStats;
    private Health health;
    private CharacterController characterController;
    private bool isLookingForTarget = true;
    private float speed => characterStats.Speed;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public void TakeDamage(int damage) => health.TakeDamage(damage);

    private void Update()
    {
        if (isLookingForTarget)
        {
            LookForTarget();
        }
        else
        {

        }
    }

    private void MoveTorvardsTarget()
    {

    }

    private void LookForTarget()
    {

    }
}
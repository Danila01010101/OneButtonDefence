using System;
using UnityEngine;

public class Health
{
    public float Amount { get; private set; }
    
    public Action<Transform> DamagReceivedFromTarget;
    public Action<float, float> HealthChanged;
    public Action Death;

    private float maxHealth;

    public Health(float startHealth)
    {
        Amount = startHealth;
        maxHealth = startHealth;
    }

    public void TakeDamage(Transform damagerTransform, int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        Amount -= damage;
        HealthChanged?.Invoke(Amount, maxHealth);
        DamagReceivedFromTarget?.Invoke(damagerTransform);

        if (Amount <= 0)
            Death?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();
        
        if (amount + Amount > maxHealth)
        {
            Amount = maxHealth;
        }        
        else
        {
            Amount += amount;
        }
        
        HealthChanged?.Invoke(Amount, maxHealth);
    }
}
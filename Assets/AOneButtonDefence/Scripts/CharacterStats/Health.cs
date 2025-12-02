using System;
using UnityEngine;

public class Health : ICharacterStat
{
    public float Value { get; private set; }
    
    public Action<Transform> DamagReceivedFromTarget;
    public Action<float, float> HealthChanged;
    public Action Death;

    private float maxHealth;

    public Health(float startHealth)
    {
        Value = startHealth;
        maxHealth = startHealth;
    }

    public void TakeDamage(Transform damagerTransform, float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        Value -= damage;
        HealthChanged?.Invoke(Value, maxHealth);
        DamagReceivedFromTarget?.Invoke(damagerTransform);

        if (Value <= 0)
            Death?.Invoke();
    }

    public void Heal(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();
        
        if (amount + Value > maxHealth)
        {
            Value = maxHealth;
        }        
        else
        {
            Value += amount;
        }
        
        HealthChanged?.Invoke(Value, maxHealth);
    }

    public void Upgrade(float amount)
    {
        maxHealth += amount;
        Heal(amount);
    }
}
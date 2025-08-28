using System;
using UnityEngine;

public class Health
{
    public float Amount { get; private set; }
    
    public Action<float, float> AmountChanged;
    public Action Death;

    private float maxHealth;

    public Health(float startHealth)
    {
        Amount = startHealth;
        maxHealth = startHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        Amount -= damage;
        AmountChanged?.Invoke(Amount, maxHealth);

        if (Amount <= 0)
            Death?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        this.Amount += amount;
    }
}
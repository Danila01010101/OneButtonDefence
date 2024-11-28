using System;

public class Health
{
    public float amount { get; private set; }
    public Action Death;

    public Health(float startHealth)
    {
        amount = startHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        amount -= damage;

        if (amount < 0)
            Death?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        this.amount += amount;
    }
}
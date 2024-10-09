public class Health
{
    public float health { get; private set; }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new System.ArgumentOutOfRangeException();

        health -= damage;
    }

    public void Heal(int amount)
    {
        if (amount < 0)
            throw new System.ArgumentOutOfRangeException();

        health += amount;
    }
}
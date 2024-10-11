public class Health
{
    public float amount { get; private set; }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new System.ArgumentOutOfRangeException();

        amount -= damage;
    }

    public void Heal(int amount)
    {
        if (amount < 0)
            throw new System.ArgumentOutOfRangeException();

        this.amount += amount;
    }
}
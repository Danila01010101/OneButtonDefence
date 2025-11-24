public class DefaultStat : ICharacterStat
{
    public DefaultStat(float value)
    {
        Value = value;
    }
    
    public float Value { get; private set; }
    
    public void Upgrade(float upgradeValue) => Value += upgradeValue;
}

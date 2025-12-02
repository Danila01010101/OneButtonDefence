public interface IStatGetter
{
    float GetStat(ResourceData.ResourceType key);
    T GetStat<T>(ResourceData.ResourceType key) where T : class, ICharacterStat;
}

public interface IStatChanger
{
    void AddStat(ResourceData.ResourceType keyForStat, ICharacterStat stat);
    void ChangeStat(ResourceData.ResourceType stat, float value);
}

public interface IStatUpgrader
{
    void AddStat(ResourceData.ResourceType keyForStat, ICharacterStat stat);
    void UpgradeStat(ResourceData.ResourceType stat, float value);
}

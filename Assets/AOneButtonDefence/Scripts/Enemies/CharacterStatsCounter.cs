using System.Collections.Generic;

public class CharacterStatsCounter : IStatGetter, IStatUpgrader
{
    private Dictionary<ResourceData.ResourceType, ICharacterStat> stats = new Dictionary<ResourceData.ResourceType, ICharacterStat>();

    public void AddStat(ResourceData.ResourceType keyForStat, ICharacterStat stat)
    {
        stats.Add(keyForStat, stat);
    }

    public float GetStat(ResourceData.ResourceType key) => stats[key].Value;
    
    public T GetStat<T>(ResourceData.ResourceType key) where T : class, ICharacterStat
    {
        return stats[key] as T;
    }


    public void UpgradeStat(ResourceData.ResourceType stat, float value)
    {
        stats[stat].Upgrade(value);
    }
}
using System.Collections.Generic;

public class CharacterStatsCounter : IStatGetter, IStatUpgrader
{
    private Dictionary<CharacterStats.StatValues, ICharacterStat> stats = new Dictionary<CharacterStats.StatValues, ICharacterStat>();

    public void AddStat(CharacterStats.StatValues keyForStat, ICharacterStat stat)
    {
        stats.Add(keyForStat, stat);
    }

    public float GetStat(CharacterStats.StatValues key) => stats[key].Value;
    
    public T GetStat<T>(CharacterStats.StatValues key) where T : class, ICharacterStat
    {
        return stats[key] as T;
    }


    public void UpgradeStat(CharacterStats.StatValues stat, float value)
    {
        stats[stat].Upgrade(value);
    }
}
public interface IStatUpgrader
{
    void AddStat(CharacterStats.StatValues keyForStat, ICharacterStat stat);
    void UpgradeStat(CharacterStats.StatValues stat, float value);
}

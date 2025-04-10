public interface IUpgradeEffectPlayer
{
    void PlayUpgradesSoundEffect(BasicBuildingData.Upgrades firstPart, BasicBuildingData.Upgrades secondPart);
    void PlayDefeatEffect();
    void PlayBattleWinEffect();
}
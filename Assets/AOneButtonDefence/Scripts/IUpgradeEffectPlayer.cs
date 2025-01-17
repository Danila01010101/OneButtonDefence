public interface IUpgradeEffectPlayer
{
    void PlayUpgradesSoundEffect(UpgradeButton.Upgrades firstPart, UpgradeButton.Upgrades secondPart);
    void PlayDefeatEffect();
    void PlayBattleWinEffect();
}
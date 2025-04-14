using UnityEngine;

public interface IUpgradeEffectPlayer
{
    AudioSource GetSource();
    void PlayUpgradesSoundEffect(BasicBuildingData.Upgrades firstPart, BasicBuildingData.Upgrades secondPart);
    void PlayDefeatEffect();
    void PlayBattleWinEffect();
}
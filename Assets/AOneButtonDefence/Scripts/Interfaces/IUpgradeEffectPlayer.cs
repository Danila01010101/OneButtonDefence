using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeEffectPlayer
{
    List<AudioSource> GetSources();
    void PlayUpgradesSoundEffect(BasicBuildingData.Upgrades firstPart, BasicBuildingData.Upgrades secondPart);
    void PlayDefeatEffect();
    void PlayBattleWinEffect();
}
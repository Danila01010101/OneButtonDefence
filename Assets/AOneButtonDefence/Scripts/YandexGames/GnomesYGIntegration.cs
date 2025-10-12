using System;
using UnityEngine;
using YG;

public class GnomesYGIntegration : IDisposable
{   
    public GnomesYGIntegration()
    {
        GameInitializer.GameInitialized += NotifyGameReady;
        GameBattleState.BattleStarted += NotifyBattleStart;
        UpgradeState.UpgradeStateStarted += NotifyBattleEnd;
        UpgradeState.UpgradeStateStarted += TryShowAd;
    }

    private void NotifyGameReady() => YG.YG2.GameReadyAPI();

    private void NotifyBattleStart() => YG.YG2.GameplayStart(); 

    private void NotifyBattleEnd() => YG.YG2.GameplayStop();

    private void TryShowAd() => YG2.InterstitialAdvShow();
    
    public void Dispose()
    {
        GameInitializer.GameInitialized -= NotifyGameReady;
        GameBattleState.BattleStarted -= NotifyBattleStart;
        UpgradeState.UpgradeStateStarted -= NotifyBattleEnd;
        UpgradeState.UpgradeStateStarted -= TryShowAd;
    }
}
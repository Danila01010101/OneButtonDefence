using System;

public class MusicPlayerMediator
{
    private IBackgroundMusicPlayer backgroundMusicPlayer;
    private IUpgradeEffectPlayer effectsPlayer;


    public MusicPlayerMediator(IBackgroundMusicPlayer backgroundMusicPlayer, IUpgradeEffectPlayer effectsPlayer)
    {
        this.backgroundMusicPlayer = backgroundMusicPlayer;
        this.effectsPlayer = effectsPlayer;
    }

    public void Subscribe()
    {
        DialogState.AnimatableDialogueStarted += backgroundMusicPlayer.StartDialogueMusic;
        GameBattleState.BattleStarted += backgroundMusicPlayer.StartBattleMusic;
        UpgradeState.UpgradeStateStarted += backgroundMusicPlayer.StartUpgradeStateMusic;
        UpgradeButton.UpgradeTypesChoosen += effectsPlayer.PlayUpgradesSoundEffect;
        GameBattleState.BattleWon += effectsPlayer.PlayBattleWinEffect;
        GameBattleState.BattleLost += effectsPlayer.PlayDefeatEffect;
        GameBattleState.BattleLost += backgroundMusicPlayer.StopMusic;
    }

    public void Unsubscribe()
    {
        DialogState.AnimatableDialogueStarted -= backgroundMusicPlayer.StartDialogueMusic;
        GameBattleState.BattleStarted -= backgroundMusicPlayer.StartBattleMusic;
        UpgradeState.UpgradeStateStarted -= backgroundMusicPlayer.StartUpgradeStateMusic;
        UpgradeButton.UpgradeTypesChoosen -= effectsPlayer.PlayUpgradesSoundEffect;
        GameBattleState.BattleWon -= effectsPlayer.PlayBattleWinEffect;
        GameBattleState.BattleLost -= effectsPlayer.PlayDefeatEffect;
        GameBattleState.BattleLost -= backgroundMusicPlayer.StopMusic;
    }
}
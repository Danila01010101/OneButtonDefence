using System;
using UnityEngine;

public class GameMusicPlayer : IBackgroundMusicPlayer, IUpgradeEffectPlayer
{
    private MusicData data;
    private AudioSource backgroundAudioSource;
    private AudioSource firstUpgradeAudioSource;
    private AudioSource secondUpgradeAudioSource;

    public GameMusicPlayer(MusicData data, AudioSource backgroundAudioSource, AudioSource firstUpgradeAudioSource, AudioSource secondUpgradeAudioSource)
    {
        this.data = data;
        this.backgroundAudioSource = backgroundAudioSource;
        this.firstUpgradeAudioSource = firstUpgradeAudioSource;
        this.secondUpgradeAudioSource = secondUpgradeAudioSource;
        backgroundAudioSource.loop = true;
        firstUpgradeAudioSource.loop = false;
        secondUpgradeAudioSource.loop = false;
    }

    public void StartLoadingMusic() => PlayMusic(backgroundAudioSource, data.LoadingSound);

    public void StartDialogueMusic() => PlayMusic(backgroundAudioSource, data.StartDialogueMusic);

    public void PlayBattleMusic() => PlayMusic(backgroundAudioSource, data.BattleMusic);

    public void PlayDefeatMusic() => PlayMusic(backgroundAudioSource, data.BattleLostSoundEffect);

    public void PlayBattleWinMusic() => PlayMusic(backgroundAudioSource, data.BattleWinSoundEffect);

    public void PlayUpgradeSoundEffect(UpgradeButton.Upgrades firstType, UpgradeButton.Upgrades secondType)
    {
        PlayMusic(firstUpgradeAudioSource, GetSoundByType(firstType));
        PlayMusic(secondUpgradeAudioSource, GetSoundByType(secondType));
    }

    private void PlayMusic(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private AudioClip GetSoundByType(UpgradeButton.Upgrades type)
    {
        switch (type)
        {
            case UpgradeButton.Upgrades.Farm:
                return data.UpgradeFarmSoundEffect;
            case UpgradeButton.Upgrades.SpiritBuilding:
                return data.UpgradeFarmSoundEffect;
            case UpgradeButton.Upgrades.MilitaryCamp:
                return data.UpgradeFarmSoundEffect;
            case UpgradeButton.Upgrades.ResourcesCenter:
                return data.UpgradeFarmSoundEffect;
            default:
                throw new NotImplementedException();
        }
    }
}
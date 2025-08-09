using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicPlayer : IBackgroundMusicPlayer, IUpgradeEffectPlayer
{
    private MusicData data;
    private AudioSource backgroundAudioSource;
    private AudioSource firstAudioSource;
    private AudioSource secondAudioSource;

    public GameMusicPlayer(MusicData data, AudioSource backgroundAudioSource, AudioSource firstAudioSource, AudioSource secondAudioSource)
    {
        this.data = data;
        this.backgroundAudioSource = backgroundAudioSource;
        this.firstAudioSource = firstAudioSource;
        this.secondAudioSource = secondAudioSource;
        backgroundAudioSource.loop = true;
        firstAudioSource.loop = false;
        secondAudioSource.loop = false;
    }
    
    public void StopMusic() => backgroundAudioSource.Stop();

    AudioSource IBackgroundMusicPlayer.GetSource() => backgroundAudioSource;

    public void StartLoadingMusic() => PlayMusic(backgroundAudioSource, data.LoadingSound);

    public void StartDialogueMusic() => PlayMusic(backgroundAudioSource, data.StartDialogueMusic);

    public void StartUpgradeStateMusic() => PlayMusic(backgroundAudioSource, data.UpgradeMusic);

    public void StartBattleMusic() => PlayMusic(backgroundAudioSource, data.BattleMusic);

    public void PlayDefeatEffect() => PlayMusic(firstAudioSource, data.BattleLostSoundEffect);

    public void PlayBattleWinEffect() => PlayMusic(firstAudioSource, data.BattleWinSoundEffect);

    List<AudioSource> IUpgradeEffectPlayer.GetSources() => new List<AudioSource>() { firstAudioSource, secondAudioSource };

    public void PlayUpgradesSoundEffect(BasicBuildingData.Upgrades firstType, BasicBuildingData.Upgrades secondType)
    {
        PlayMusic(firstAudioSource, GetSoundByType(firstType));
        PlayMusic(secondAudioSource, GetSoundByType(secondType));
    }

    private void PlayMusic(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private AudioClip GetSoundByType(BasicBuildingData.Upgrades type)
    {
        switch (type)
        {
            case BasicBuildingData.Upgrades.Farm:
                return data.UpgradeFarmSoundEffect;
            case BasicBuildingData.Upgrades.SpiritBuilding:
                return data.UpgradeChurchSoundEffect;
            case BasicBuildingData.Upgrades.MilitaryCamp:
                return data.UpgradeCampSoundEffect;
            case BasicBuildingData.Upgrades.Factory:
                return data.UpgradeFactorySoundEffect;
            case BasicBuildingData.Upgrades.WarriorStrength:
                return data.WarriorBuffSoundEffect;
            case BasicBuildingData.Upgrades.SpellStrength:
                return data.SpellBuffSoundEffect;
            default:
                throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicPlayer : IBackgroundMusicPlayer, IUpgradeEffectPlayer, IDisposable
{
    private MusicData data;
    private AudioSource backgroundAudioSource;
    private AudioSource firstAudioSource;
    private AudioSource secondAudioSource;

    private Coroutine currentStopCoroutine;

    public GameMusicPlayer(MusicData data, AudioSource backgroundAudioSource, AudioSource firstAudioSource, AudioSource secondAudioSource)
    {
        this.data = data;
        this.backgroundAudioSource = backgroundAudioSource;
        this.firstAudioSource = firstAudioSource;
        this.secondAudioSource = secondAudioSource;
        
        backgroundAudioSource.Play();
        backgroundAudioSource.loop = true;
        firstAudioSource.loop = false;
        secondAudioSource.loop = false;

        SkinOpenSoundPlayer.SkinOpened += OnSkinOpened;
        SkinPanel.ShopDisabled += OnShopDisabled;
    }

    private void OnSkinOpened(float clipLenght)
    {
        if (currentStopCoroutine != null)
        {
            CoroutineStarter.Instance.StopCoroutine(currentStopCoroutine);
        }
        
        if (backgroundAudioSource.isPlaying)
            backgroundAudioSource.Pause();

        currentStopCoroutine = CoroutineStarter.Instance.StartCoroutine(ReturnBackgroundMusic(clipLenght));
    }

    private void OnShopDisabled()
    {
        if (currentStopCoroutine != null && CoroutineStarter.Instance != null)
        {
            CoroutineStarter.Instance.StopCoroutine(currentStopCoroutine);
            currentStopCoroutine = null;
        }
        
        if (backgroundAudioSource != null && !backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.UnPause();
        }
    }

    private System.Collections.IEnumerator ReturnBackgroundMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        backgroundAudioSource.UnPause();
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

    public void PlayUpgradesSoundEffect(BasicBuildingData.Upgrades firstType)
    {
        PlayMusic(firstAudioSource, GetSoundByType(firstType));
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
            case BasicBuildingData.Upgrades.Farm: return data.UpgradeFarmSoundEffect;
            case BasicBuildingData.Upgrades.SpiritBuilding: return data.UpgradeChurchSoundEffect;
            case BasicBuildingData.Upgrades.MilitaryCamp: return data.UpgradeCampSoundEffect;
            case BasicBuildingData.Upgrades.Factory: return data.UpgradeFactorySoundEffect;
            case BasicBuildingData.Upgrades.BuildingEffectiveness: return data.UpgradeFactorySoundEffect;
            case BasicBuildingData.Upgrades.WarriorStrength: return data.WarriorBuffSoundEffect;
            case BasicBuildingData.Upgrades.SpellStrength: return data.SpellBuffSoundEffect;
            case BasicBuildingData.Upgrades.ArmorTower: return data.WarriorBuffSoundEffect;
            case BasicBuildingData.Upgrades.WarriorSpeed: return data.WarriorBuffSoundEffect;
            case BasicBuildingData.Upgrades.RangeWarriors: return data.UpgradeRangeWarriorsSoundEffect;
            case BasicBuildingData.Upgrades.HealingTower: return data.UpgradeHealingTowerSoundEffect;
            case BasicBuildingData.Upgrades.DefenceTower: return data.UpgradeDefenceTowerSoundEffect;
            default: throw new NotImplementedException();
        }
    }

    public void Dispose()
    {
        SkinOpenSoundPlayer.SkinOpened -= OnSkinOpened;
        SkinPanel.ShopDisabled -= OnShopDisabled;
    }
}
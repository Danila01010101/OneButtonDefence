using UnityEngine;
using System;
using System.Collections;

public class MusicPlayerInitializer : IGameInitializerStep, IDisposable
{
    private Transform _parent;
    private MusicData _musicData;
    private GameMusicPlayer _musicPlayer;
    public IBackgroundMusicPlayer BackgroundPlayer { get; private set; }
    public IUpgradeEffectPlayer UpgradeEffectPlayer { get; private set; }

    public MusicPlayerInitializer(Transform parent, MusicData musicData)
    {
        _parent = parent;
        _musicData = musicData;
    }

    public IEnumerator Initialize()
    {
        var go = new GameObject("MusicPlayer");
        go.transform.SetParent(_parent);
        var backgroundPlayer = go.AddComponent<AudioSource>();
        backgroundPlayer.volume = 0.6f;
        var firstUpgradePlayer = go.AddComponent<AudioSource>();
        firstUpgradePlayer.volume = 0.8f;
        var secondUpgradePlayer = go.AddComponent<AudioSource>();
        secondUpgradePlayer.volume = 0.8f;
        _musicPlayer = new GameMusicPlayer(_musicData, backgroundPlayer, firstUpgradePlayer, secondUpgradePlayer);
        BackgroundPlayer = _musicPlayer;
        UpgradeEffectPlayer = _musicPlayer;
        yield break;
    }

    public void Dispose()
    {
        _musicPlayer.Dispose();
    }
}
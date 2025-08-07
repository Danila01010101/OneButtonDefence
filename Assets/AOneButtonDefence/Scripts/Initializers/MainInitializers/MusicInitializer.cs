using System.Collections;
using UnityEngine;

public class MusicInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private MusicData musicData;

    public static IBackgroundMusicPlayer BackgroundPlayer { get; private set; }
    public static IUpgradeEffectPlayer UpgradeEffectPlayer { get; private set; }

    private MusicPlayerMediator _mediator;

    public IEnumerator Initialize()
    {
        var go = new GameObject("MusicPlayer");
        go.transform.SetParent(transform);

        var background = go.AddComponent<AudioSource>();
        var upgrade1 = go.AddComponent<AudioSource>();
        var upgrade2 = go.AddComponent<AudioSource>();

        var player = new GameMusicPlayer(musicData, background, upgrade1, upgrade2);
        BackgroundPlayer = player;
        UpgradeEffectPlayer = player;

        _mediator = new MusicPlayerMediator(BackgroundPlayer, UpgradeEffectPlayer);
        _mediator.Subscribe();

        BackgroundPlayer.StartLoadingMusic();

        yield return null;
    }

    private void OnDestroy()
    {
        _mediator?.Unsubscribe();
    }
}
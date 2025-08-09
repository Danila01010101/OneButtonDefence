using System.Collections;

public class MusicMediatorInitializer : IGameInitializerStep
{
    private IBackgroundMusicPlayer _bgPlayer;
    private IUpgradeEffectPlayer _upgradePlayer;
    public MusicPlayerMediator Mediator { get; private set; }

    public MusicMediatorInitializer(IBackgroundMusicPlayer bg, IUpgradeEffectPlayer upgrade)
    {
        _bgPlayer = bg;
        _upgradePlayer = upgrade;
    }

    public IEnumerator Initialize()
    {
        Mediator = new MusicPlayerMediator(_bgPlayer, _upgradePlayer);
        Mediator.Subscribe();
        yield break;
    }
}
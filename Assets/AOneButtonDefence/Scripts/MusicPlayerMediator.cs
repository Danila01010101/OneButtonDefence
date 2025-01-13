public class MusicPlayerMediator
{
    private IBackgroundMusicPlayer backgroundMusicPlayer;
    private IUpgradeEffectPlayer upgradeEffectPlayer;


    public MusicPlayerMediator(IBackgroundMusicPlayer backgroundMusicPlayer, IUpgradeEffectPlayer upgradeEffectPlayer)
    {
        this.backgroundMusicPlayer = backgroundMusicPlayer;
        this.upgradeEffectPlayer = upgradeEffectPlayer;
    }
}
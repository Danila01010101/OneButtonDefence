public interface IBackgroundMusicPlayer
{
    void StartLoadingMusic();
    void StartDialogueMusic();
    void StartUpgradeStateMusic();
    void StartBattleMusic();
    void StopMusic();
}
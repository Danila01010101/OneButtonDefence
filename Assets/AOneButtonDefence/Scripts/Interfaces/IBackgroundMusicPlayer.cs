using UnityEngine;

public interface IBackgroundMusicPlayer
{
    AudioSource GetSource();
    void StartLoadingMusic();
    void StartDialogueMusic();
    void StartUpgradeStateMusic();
    void StartBattleMusic();
    void StopMusic();
}
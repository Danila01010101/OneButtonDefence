using System;
using UnityEngine;

public class SkinOpenSoundPlayer : IDisposable
{
    private AudioSource audioSource;

    public SkinOpenSoundPlayer(AudioSource source)
    {
        audioSource = source;
        AudioSettings.AddAudioSource(audioSource);
        SkinPanel.SkinBought += PlaySound;
    }

    public void PlaySound(SkinData skinData)
    {
        audioSource.clip = skinData.UnlockMusic;
        audioSource.Play();
    }

    public void Dispose()
    {
        SkinPanel.SkinBought -= PlaySound;
    }
}
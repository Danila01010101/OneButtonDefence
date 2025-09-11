using System;
using UnityEngine;

public class SkinOpenSoundPlayer : IDisposable
{
    private AudioSource audioSource;
    
    public static event Action<float> SkinOpened;

    public SkinOpenSoundPlayer(AudioSource source)
    {
        audioSource = source;
        AudioSettings.AddAudioSource(audioSource);
        SkinPanel.SkinChanged += PlaySound;
    }

    public void PlaySound(SkinData skinData)
    {
        audioSource.clip = skinData.UnlockMusic;
        audioSource.Play();
        
        SkinOpened.Invoke(audioSource.clip.length);
    }

    public void Dispose()
    {
        SkinPanel.SkinBought -= PlaySound;
    }
}
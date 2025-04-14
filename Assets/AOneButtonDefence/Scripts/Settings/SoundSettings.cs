using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    
    private List<AudioSource> registeredAudioSources;

    public static Action<GameObject> SettingsInitialized;
    
    public float value { get; private set; }
    
    private static SoundSettings instance;

    public void Initialize(List<AudioSource> startAudioSources)
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        foreach (var source in startAudioSources)
        {
            registeredAudioSources.Add(source);
        }

        volumeSlider.onValueChanged.AddListener(UpdateSources);
        SettingsInitialized?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
    
    private void UpdateSources(float newValue)
    {
        value = newValue;
        
        foreach (AudioSource audioSource in registeredAudioSources)
        {
            audioSource.volume = value;
        }
    }

    public void VolumeToZero()
    {
        volumeSlider.value = (value == 0) ? 1 : 0;
    }
    
    public static void AddAudioSource(AudioSource audioSource) => instance.registeredAudioSources.Add(audioSource);

    public void SettingsClose()
    {
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [FormerlySerializedAs("soundButton")] [SerializeField] private Image soundButtonImage;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Sprite volumeIconOn;
    [SerializeField] private Sprite volumeIconOff;
    
    private List<AudioSource> registeredAudioSources = new List<AudioSource>();
    
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
    }
    
    private void UpdateSources(float newValue)
    {
        value = newValue;
        soundButtonImage.sprite = (value == 0) ? volumeIconOn : volumeIconOff;
        
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
}

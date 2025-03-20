using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField]private Slider volumeSlider;
    
    private AudioSource[] musicAudioSource;
    
    public float value;
    void Start()
    {
        musicAudioSource = GameObject.Find("MusicPlayer").GetComponents<AudioSource>();
    }
    
    void Update()
    {
        value = volumeSlider.value;
        
        foreach (AudioSource audioSource in musicAudioSource)
        {
            audioSource.volume = value;
        }
    }

    public void VolumeToZero()
    {
        volumeSlider.value = (value == 0) ? 1 : 0;
    }
}

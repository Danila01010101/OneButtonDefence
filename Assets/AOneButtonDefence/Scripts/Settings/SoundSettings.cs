using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField]private Slider volumeSlider;
    
    [HideInInspector]
    public static Action<GameObject> SettingsInitialized;
    
    private AudioSource[] musicAudioSource;
    
    public float value;

    public void Initialize()
    {
        SettingsInitialized?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
    
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

    public void SettingsClose()
    {
        gameObject.SetActive(false);
    }
}

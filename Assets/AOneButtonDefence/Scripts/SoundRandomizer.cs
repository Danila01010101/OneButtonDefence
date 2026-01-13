using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomizer : MonoBehaviour
{
    public SoundWithChance[] sounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (sounds == null || sounds.Length == 0)
            Debug.LogWarning("Массив звуков пуст!");
    }

    public void PlayRandomSound()
    {
        float totalChance = 0f;
        foreach (var sound in sounds)
        {
            totalChance += sound.chance;
        }

        float randomValue = Random.Range(0f, totalChance);
        foreach (var sound in sounds)
        {
            if (randomValue <= sound.chance)
            {
                audioSource.clip = sound.clip;
                audioSource.Play();
                return;
            }
            else
            {
                randomValue -= sound.chance;
            }
        }
    }
    
}

[System.Serializable]
public class SoundWithChance
{
    public AudioClip clip;
    [Range(0, 1)] public float chance;
}

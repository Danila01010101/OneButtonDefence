using UnityEngine;

public static class AudioUtility
{
    public static void PlayClipAtPoint(AudioSource source, Vector3 position, AudioClip clip = null)
    {
        if (source == null) return;
        if (clip == null) clip = source.clip;
        if (clip == null) return;

        GameObject go = new GameObject("TempAudio");
        go.transform.position = position;
        AudioSource newSource = go.AddComponent<AudioSource>();

        CopySettings(source, newSource);

        newSource.clip = clip;
        newSource.Play();

        Object.Destroy(go, clip.length / newSource.pitch);
    }

    private static void CopySettings(AudioSource from, AudioSource to)
    {
        to.outputAudioMixerGroup = from.outputAudioMixerGroup;
        to.mute = from.mute;
        to.bypassEffects = from.bypassEffects;
        to.bypassListenerEffects = from.bypassListenerEffects;
        to.bypassReverbZones = from.bypassReverbZones;
        to.priority = from.priority;
        to.volume = from.volume;
        to.pitch = from.pitch;
        to.panStereo = from.panStereo;
        to.spatialBlend = from.spatialBlend;
        to.reverbZoneMix = from.reverbZoneMix;
        to.dopplerLevel = from.dopplerLevel;
        to.spread = from.spread;
        to.rolloffMode = from.rolloffMode;
        to.minDistance = from.minDistance;
        to.maxDistance = from.maxDistance;
        to.ignoreListenerVolume = from.ignoreListenerVolume;
        to.ignoreListenerPause = from.ignoreListenerPause;
        to.loop = false;
        to.playOnAwake = false;
    }
}
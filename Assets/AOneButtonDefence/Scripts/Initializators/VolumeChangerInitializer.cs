using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VolumeChangerInitializer : IGameInitializerStep
{
    private GameplayCanvas _canvas;
    private List<AudioSource> _sources;
    private float _startVolume;

    public VolumeChangerInitializer(GameplayCanvas canvas, List<AudioSource> sources, float startVolume)
    {
        _canvas = canvas;
        _sources = sources;
        _startVolume = startVolume;
    }

    public IEnumerator Initialize()
    {
        _canvas?.AudioSettings.Initialize(_sources, _startVolume);
        yield break;
    }
}
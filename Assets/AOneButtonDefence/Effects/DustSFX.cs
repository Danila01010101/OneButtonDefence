using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DustSFX : MonoBehaviour
{
    public float Time = 1f;
    private ParticleSystem _particleSystem;
    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        var main = _particleSystem.main;
        main.startLifetime = Time;
        _particleSystem.Stop(false);
    }
    
}

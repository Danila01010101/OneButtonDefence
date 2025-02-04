using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellData SpellData;

    private ParticleSystem _circle;
    private ParticleSystem _sides;
    private ParticleSystem _light;
    private ParticleSystem _sparks;
    private ParticleSystem _flakes;
    private void Start()
    {
        Initialize(SpellData);
    }
    public void Initialize(SpellData spellData)
    {
        GetPrivateComponents();
        if (spellData.FineTuning)
        {

        }
        ChangeColor(spellData);
    }

    private void GetPrivateComponents()
    {
        _circle = GetComponent<ParticleSystem>();
        _sides = transform.GetChild(0).GetComponent<ParticleSystem>();
        _light = transform.GetChild(1).GetComponent<ParticleSystem>();
        _sparks = transform.GetChild(2).GetComponent<ParticleSystem>();
        _flakes = transform.GetChild(3).GetComponent<ParticleSystem>();
    }

    private void ChangeColor(SpellData spellData)
    {
        var mainModuleCircle = _circle.main;
        var mainModuleSides = _sides.main;
        var mainModuleLight = _light.main;
        var mainModuleSpark = _sparks.main;
        var mainModuleFlakes = _flakes.main;
        
        if (spellData.FineTuning == true)
        {
            mainModuleCircle.startColor = spellData.CircleColor;
            mainModuleSides.startColor = spellData.SidesColor;
            mainModuleLight.startColor = spellData.LightColor;
            mainModuleSpark.startColor = spellData.SparksColor;
            mainModuleFlakes.startColor = spellData.FlakesColor;
        }
        else
        {
            mainModuleCircle.startColor = spellData.MainColor;
            mainModuleSides.startColor = spellData.SidesColor;
            mainModuleLight.startColor = spellData.MainColor;
            mainModuleSpark.startColor = spellData.SidesColor;
            mainModuleFlakes.startColor = spellData.MainColor;
        }

        _circle.Play();
        _sides.Play();
        _light.Play();
        _sparks.Play();
        _flakes.Play();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private ParticleSystem _circle;
    private ParticleSystem _sides;
    private ParticleSystem _light;
    private ParticleSystem _sparks;
    private ParticleSystem _flakes;

    private float leftTime;
    private SpellData spell;
    private HashSet<IDamagable> targets = new HashSet<IDamagable>();
    public void Initialize(SpellData spellData)
    {
        spell = spellData;
        GetPrivateComponents();
        ChangeColor(spellData);
        ChangeParticles(spellData);
        StartParticlesSystem();
        Invoke("Destroy", spell.Time);
    }
    private void Update()
    {
        Timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null)
        {
            targets.Add(other.GetComponent<IDamagable>());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null)
        {
            targets.Remove(other.GetComponent<IDamagable>());
        }
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

    }

    private void ChangeParticles(SpellData spellData)
    {
        ParticleSystemRenderer mainModuleFlakes = _flakes.GetComponent<ParticleSystemRenderer>();
        mainModuleFlakes.material.SetTexture("_baseMap", spellData.EffectParticles);
    }

    private void StartParticlesSystem()
    {
        _circle.Play();
        _sides.Play();
        _light.Play();
        _sparks.Play();
        _flakes.Play();
    }

    private void Timer()
    {
        leftTime += Time.deltaTime;
        if (leftTime > spell.TimePerDamage) 
        {
            leftTime -= spell.TimePerDamage;
            List<IDamagable> list = new List<IDamagable>(targets);
            foreach (IDamagable target in list) 
            {
                if (target.IsAlive())
                {
                    target.TakeDamage(spell.Damage);
                }
                else 
                {
                    targets.Remove(target);
                }
            }
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}

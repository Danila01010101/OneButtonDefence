using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private ParticleSystem _circle;
    private ParticleSystem _sides;
    private ParticleSystem _light;
    private ParticleSystem _sparks;
    private ParticleSystem _flakes;

    private ParticleSystem.MainModule _circleMain;
    private ParticleSystem.MainModule _sidesMain;
    private ParticleSystem.MainModule _lightMain;
    private ParticleSystem.MainModule _sparksMain;
    private ParticleSystem.MainModule _flakesMain;

    private float leftTime;
    private LayerMask targetLayer;
    private SpellData spell;
    private HashSet<IDamagable> targets = new HashSet<IDamagable>();

    public void Initialize(SpellData spellData, LayerMask damagableTargetLayer)
    {
        targetLayer = damagableTargetLayer;
        spell = spellData;
        GetPrivateComponents();
        StopParticleSystem();
        ChangeColor(spellData);
        ChangeParticles(spellData);
        ChangeLifeTime(spellData.Time);
        StartParticlesSystem();
        Invoke(nameof(Destroy), spell.Time);
    }

    private void Update()
    {
        Timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null && (targetLayer & (1 << other.gameObject.layer)) != 0);
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

        _circleMain = _circle.main;
        _sidesMain = _sides.main;
        _lightMain = _light.main;
        _sparksMain = _sparks.main;
        _flakesMain = _flakes.main;
    }

    private void ChangeColor(SpellData spellData)
    {
        if (spellData.FineTuning == true)
        {
            _circleMain.startColor = spellData.CircleColor;
            _sidesMain.startColor = spellData.SidesColor;
            _lightMain.startColor = spellData.LightColor;
            _sparksMain.startColor = spellData.SparksColor;
            _flakesMain.startColor = spellData.FlakesColor;
        }
        else
        {
            _circleMain.startColor = spellData.MainColor;
            _sidesMain.startColor = spellData.SidesColor;
            _lightMain.startColor = spellData.MainColor;
            _sparksMain.startColor = spellData.SidesColor;
            _flakesMain.startColor = spellData.MainColor;
        }

    }

    private void ChangeParticles(SpellData spellData)
    {
        ParticleSystemRenderer mainModuleFlakes = _flakes.GetComponent<ParticleSystemRenderer>();
        mainModuleFlakes.material.SetTexture("_baseMap", spellData.EffectParticles);
    }

    private void ChangeLifeTime(float time)

    {
        _circleMain.startLifetime = time;
        _sidesMain.startLifetime = time;
        _lightMain.startLifetime = time;
        _sparksMain.startLifetime = time;
        _flakesMain.startLifetime = time;

        _circleMain.duration = time;
        _sidesMain.duration = time;
        _lightMain.duration = time;
        _sparksMain.duration = time;
        _flakesMain.duration = time;
    }

    private void StopParticleSystem()
    {
        _circle.Stop();
        _sides.Stop();
        _light.Stop();
        _sparks.Stop();
        _flakes.Stop();
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
                if (target != null && target.IsAlive())
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

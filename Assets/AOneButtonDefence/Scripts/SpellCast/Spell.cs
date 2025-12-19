using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour, IDamagable, IEffectActivator
{
    [SerializeField] private ParticleSystem _circle;
    [SerializeField] private ParticleSystem _sides;
    [SerializeField] private ParticleSystem _light;
    [SerializeField] private ParticleSystem _sparks;
    [SerializeField] private ParticleSystem _flakes;

    private ParticleSystem.MainModule _circleMain;
    private ParticleSystem.MainModule _sidesMain;
    private ParticleSystem.MainModule _lightMain;
    private ParticleSystem.MainModule _sparksMain;
    private ParticleSystem.MainModule _flakesMain;

    private float leftTime;
    private float damageModificator;
    private StartResourceAmount effect;
    private LayerMask targetLayer;
    private SpellData spell;
    private IDamagable selfDamageable;
    private HashSet<IDamagable> targets = new HashSet<IDamagable>();

    public void Initialize(SpellData spellData, LayerMask damagableTargetLayer, float damageModificator)
    {
        effect = spellData.Effect;
        this.damageModificator = damageModificator;
        targetLayer = damagableTargetLayer;
        spell = spellData;
        GetPrivateComponents();
        StopParticleSystem();
        ChangeColor(spellData);
        ChangeParticles(spellData);
        ChangeLifeTime(spellData.Time);
        StartParticlesSystem();
        selfDamageable = this;
        Invoke(nameof(Destroy), spell.Time);
    }

    private void Update()
    {
        Timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null && (targetLayer & (1 << other.gameObject.layer)) != 0)
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
        if (_circle == null) _circle = GetComponent<ParticleSystem>();
        if (_sides == null) _sides = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (_light == null) _light = transform.GetChild(1).GetComponent<ParticleSystem>();
        if (_sparks == null) _sparks = transform.GetChild(2).GetComponent<ParticleSystem>();
        if (_flakes == null) _flakes = transform.GetChild(3).GetComponent<ParticleSystem>();

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
                if (target != null && target.GetTransform() != null && target.IsAlive())
                {
                    if (spell.EffectForEveryEnemy != null)
                        Instantiate(spell.EffectForEveryEnemy, target.GetTransform().position + spell.EffectForEveryEnemy.transform.localPosition, Quaternion.identity);
                    target.TakeDamage(selfDamageable, spell.Damage * ((100 + damageModificator)/100));
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

    public bool IsAlive() => false;

    public void TakeDamage(IDamagable damagerTransform, float damage) { }

    public Transform GetTransform() => transform;

    public string GetName() => gameObject.name;

    public Building.EffectCastInfo GetEffectInfo()
    {
        return new Building.EffectCastInfo(effect, transform.position);
    }
}

using AOneButtonDefence.Scripts.StateMachine;
using UnityEngine;

public class EffectReceiver
{
    public IEffectsHandler EffectsHandler { get; private set; }
    private readonly CharacterStatsCounter characterStatsCounter;
    private readonly Transform selfTransform;

    public EffectReceiver(IEffectsHandler handler, Transform selfTransform, CharacterStatsCounter characterStatsCounter)
    {
        EffectsHandler = handler;
        this.characterStatsCounter = characterStatsCounter;
        this.selfTransform = selfTransform;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IEffectActivator>(out var activator))
            return;

        var info = activator.GetEffectInfo();
        var prefab = info.BuffResource?.Resource?.ResourceEffect;

        ActiveEffect activeEffect = null;

        if (prefab != null)
        {
            var instance = GameObject.Instantiate(prefab, selfTransform);
            instance.transform.localPosition = Vector3.zero;
            activeEffect = new ActiveEffect(info, activator, instance, characterStatsCounter);
        }

        EffectsHandler?.AddEffect(activeEffect);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<IEffectActivator>(out var activator))
            return;

        EffectsHandler?.RemoveEffectByActivator(activator);
    }
}
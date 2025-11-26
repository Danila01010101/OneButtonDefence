using System.Collections.Generic;
using AOneButtonDefence.Scripts.StateMachine;
using DG.Tweening;
using UnityEngine;

public class PlayerEffectsHandler : MonoBehaviour, IEffectsHandler
{
    public List<ActiveEffect> CurrentEffects { get; private set; } = new();
    public Vector3 OriginalScale { get; private set; }
    public CharacterStatsCounter Stats { get; private set; }
    public Transform SelfTransform => transform;

    private Tween scaleTween;

    public void Initialize(CharacterStatsCounter stats)
    {
        Stats = stats;
        OriginalScale = transform.localScale;
    }

    public void AddEffect(ActiveEffect effect)
    {
        if (effect == null) return;
        CurrentEffects.Add(effect);
        effect.Enable();
        RecalcScale();
    }

    public void RemoveEffect(ActiveEffect effect)
    {
        if (effect == null) return;

        effect.Disable();
        CurrentEffects.Remove(effect);

        if (effect.EffectInstance != null)
            Destroy(effect.EffectInstance.gameObject);

        RecalcScale();
    }

    public void RemoveEffectByActivator(IEffectActivator activator)
    {
        var e = CurrentEffects.Find(x => x.OriginActivator == activator);
        if (e != null)
            RemoveEffect(e);
    }
    
    public void ResetAllEffects()
    {
        foreach (var e in CurrentEffects)
        {
            e.Disable();

            if (e.EffectInstance != null)
                Destroy(e.EffectInstance.gameObject);
        }

        CurrentEffects.Clear();

        if (scaleTween != null)
            scaleTween.Kill();

        transform.localScale = OriginalScale;
    }

    private void RecalcScale()
    {
        float total = 0;
        foreach (var e in CurrentEffects) total += e.ScaleMultiplier;

        if (scaleTween != null) scaleTween.Kill();

        scaleTween = transform.DOScale(OriginalScale + Vector3.one * total, 0.25f)
            .SetEase(Ease.OutQuad);
    }
}
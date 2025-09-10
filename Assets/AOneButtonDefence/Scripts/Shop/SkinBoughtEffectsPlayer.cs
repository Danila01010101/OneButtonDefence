using System;
using UnityEngine;
using DG.Tweening;

public class SkinBoughtEffectsPlayer : IDisposable
{
    private readonly ParticleSystem particles;
    private readonly Transform modelTransform;

    public SkinBoughtEffectsPlayer(ParticleSystem particles, Transform modelTransform)
    {
        this.particles = particles;
        this.modelTransform = modelTransform;

        SkinPanel.SkinBought += PlayEffect;
    }

    private void PlayEffect(SkinData data)
    {
        if (particles != null)
            particles.Play();

        if (modelTransform != null)
        {
            Sequence seq = DOTween.Sequence();

            Vector3 startPos = modelTransform.localPosition;
            float jumpHeight = 0.3f;
            float sideShift = 0.2f;
            float duration = 0.2f;

            seq.Append(modelTransform.DOLocalMove(startPos + new Vector3(sideShift, jumpHeight, 0), duration).SetEase(Ease.OutQuad));
            seq.Append(modelTransform.DOLocalMove(startPos + new Vector3(-sideShift, jumpHeight, 0), duration).SetEase(Ease.OutQuad));
            seq.Append(modelTransform.DOLocalMove(startPos + new Vector3(0, jumpHeight, sideShift), duration).SetEase(Ease.OutQuad));
            seq.Append(modelTransform.DOLocalMove(startPos + new Vector3(0, jumpHeight, -sideShift), duration).SetEase(Ease.OutQuad));

            seq.Append(modelTransform.DOLocalMove(startPos, duration).SetEase(Ease.InQuad));
        }
    }

    public void Dispose()
    {
        SkinPanel.SkinBought -= PlayEffect;
    }
}
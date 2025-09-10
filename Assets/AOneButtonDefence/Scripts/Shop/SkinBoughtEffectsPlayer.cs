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
            Vector3 startRot = modelTransform.localEulerAngles;

            float sideShift = 0.2f;
            float upShift = 0.2f;
            float duration = 0.25f;
            
            seq.AppendInterval(0.6f);

            seq.Append(modelTransform.DOLocalMove(startPos + new Vector3(-sideShift, upShift, 0), duration)
                .SetEase(Ease.OutQuad));
            seq.Join(modelTransform.DOLocalRotate(startRot + new Vector3(0, 0, 30f), duration)
                .SetEase(Ease.OutQuad));

            seq.Append(modelTransform.DOLocalMove(startPos, duration).SetEase(Ease.InOutQuad));
            seq.Join(modelTransform.DOLocalRotate(startRot, duration).SetEase(Ease.InOutQuad));

            seq.Append(modelTransform.DOLocalMove(startPos + new Vector3(sideShift, upShift, 0), duration)
                .SetEase(Ease.OutQuad));
            seq.Join(modelTransform.DOLocalRotate(startRot + new Vector3(0, 0, -30f), duration)
                .SetEase(Ease.OutQuad));

            seq.Append(modelTransform.DOLocalMove(startPos, duration).SetEase(Ease.InOutQuad));
            seq.Join(modelTransform.DOLocalRotate(startRot, duration).SetEase(Ease.InOutQuad));

            seq.Append(modelTransform.DOLocalMoveY(startPos.y + 0.25f, 0.35f)
                .SetEase(Ease.OutQuad));
            seq.Append(modelTransform.DOLocalMoveY(startPos.y, 0.4f)
                .SetEase(Ease.OutBounce));
        }
    }

    public void Dispose()
    {
        SkinPanel.SkinBought -= PlayEffect;
    }
}
using System;
using UnityEngine;

public interface IEnemyReward
{
    GameObject GameObject { get; }
    void FlyToUI(Camera uICamera, RectTransform uITarget, float duration, float endScale, Action onComplete = null);
    void BounceAside(System.Action onComplete);
    void PlayAppearEffect();
    void Destroy();
}
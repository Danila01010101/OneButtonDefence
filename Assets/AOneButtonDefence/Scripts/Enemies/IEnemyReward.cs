using UnityEngine;

public interface IEnemyReward
{
    GameObject GameObject { get; }
    void FlyToUI(Vector3 startWorldPosition, RectTransform uiTarget, Canvas canvas, System.Action onComplete);
    void BounceAside(System.Action onComplete);
    void PlayAppearEffect();
    void Destroy();
}
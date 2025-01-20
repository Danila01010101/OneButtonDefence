using UnityEngine;

public interface IEnemyReward
{
    FlyToUI UIAnimator { get; }
    GameObject GameObject { get; }
    void PlayEffect();
    void Destroy();
}
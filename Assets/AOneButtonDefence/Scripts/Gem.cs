using System;
using UnityEngine;

[RequireComponent(typeof(FlyToUI))]
[RequireComponent(typeof(DropBounceEffect))]
[RequireComponent(typeof(AnimationScript))]
public class Gem : MonoBehaviour, IEnemyReward
{
    [SerializeField] private ParticleSystem summonEffect;
    
    private DropBounceEffect dropBounceEffect;
    private AnimationScript idleAnimation;
    private FlyToUI uiAnimator;
    
    public GameObject GameObject => gameObject;

    private void Awake()
    {
        uiAnimator = GetComponent<FlyToUI>();
        dropBounceEffect = GetComponent<DropBounceEffect>();
        idleAnimation = GetComponent<AnimationScript>();
        EnableIdleAnimation();
    }

    public void FlyToUI(Camera uICamera, RectTransform uITarget, float duration, float endScale, Action onComplete = null) =>
        uiAnimator.Fly(uITarget, uICamera, duration, endScale, onComplete);

    public void BounceAside(Action onComplete)
    {
        DisableIdleAnimation();
        dropBounceEffect.StartBounce(transform.position, () =>
        {
            EnableIdleAnimation();
            onComplete.Invoke();
        });
    }
    
    public void PlayAppearEffect() => Instantiate(summonEffect, transform.position, Quaternion.identity, Camera.main.transform);
    
    public void Destroy() => Destroy(gameObject);
    
    private void EnableIdleAnimation() => idleAnimation.enabled = true;
    
    private void DisableIdleAnimation() => idleAnimation.enabled = false;
}
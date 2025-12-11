using System;
using DG.Tweening;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SphereCollider))]
public class RadiusVisualizer : MonoBehaviour
{
    [SerializeField] private Transform radiusObject;
    [SerializeField] private float animationDuration = 0.75f;
    [SerializeField] private float delay = 1f;
    
    private SphereCollider sphereCollider;
    private float initialRadius;
    private Vector3 initialScale;
    private Tween currentTween;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (radiusObject == null)
        {
            Debug.LogWarning($"{nameof(RadiusVisualizer)}: radiusObject не назначен!", this);
            return;
        }

        initialRadius = sphereCollider.radius;
        initialScale = new Vector3(initialRadius, radiusObject.localScale.y, initialRadius);
        radiusObject.localScale = new Vector3(0f, radiusObject.localScale.y, 0f);
        sphereCollider.enabled = false;
    }

    private void EnableRadiusEffect()
    {
        currentTween?.Kill();
        sphereCollider.enabled = true;
        sphereCollider.radius = 0f;

        var startScale = new Vector3(0f, radiusObject.localScale.y, 0f);
        radiusObject.localScale = startScale;

        currentTween = DOVirtual.DelayedCall(delay, () =>
        {
            DOTween.To(() => sphereCollider.radius, r => sphereCollider.radius = r, initialRadius, animationDuration)
                .SetEase(Ease.OutQuad);

            DOTween.To(
                () => radiusObject.localScale.x,
                x =>
                {
                    radiusObject.localScale = new Vector3(x, radiusObject.localScale.y, x);
                },
                initialScale.x,
                animationDuration
            ).SetEase(Ease.OutQuad);
        });
    }

    private void DisableRadiusEffect()
    {
        currentTween?.Kill();

        currentTween = DOVirtual.DelayedCall(delay, () =>
        {
            DOTween.To(() => sphereCollider.radius, r => sphereCollider.radius = r, 0f, animationDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => sphereCollider.enabled = false);

            DOTween.To(
                () => radiusObject.localScale.x,
                x =>
                {
                    radiusObject.localScale = new Vector3(x, radiusObject.localScale.y, x);
                },
                0f,
                animationDuration
            ).SetEase(Ease.InQuad);
        });
    }

    private void OnEnable()
    {
        GameBattleState.BattleStarted += EnableRadiusEffect;
        GameBattleState.BattleWon += DisableRadiusEffect;
        GameBattleState.BattleLost += DisableRadiusEffect;
    }

    private void OnDisable()
    {
        GameBattleState.BattleStarted -= EnableRadiusEffect;
        GameBattleState.BattleWon -= DisableRadiusEffect;
        GameBattleState.BattleLost -= DisableRadiusEffect;
    }
}
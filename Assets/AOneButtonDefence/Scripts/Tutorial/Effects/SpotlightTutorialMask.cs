using UnityEngine;
using DG.Tweening;

public class SpotlightTutorialMask : MonoBehaviour
{
    [SerializeField] private Material material;
    
    [Header("Скорость анимации")]
    public float animationDuration = 0.4f;

    [Header("Отступ от объекта")]
    public float padding = 20f;

    [Header("Маска, к которой применён шейдер")]
    public Material spotlightMaterial;

    [Header("Камера канваса (если нужна)")]
    public Camera uiCamera;

    private Tween moveTween;
    private Tween sizeTween;

    public void SetNewTarget(RectTransform target)
    {
        Vector2 canvasSize = ((RectTransform)transform).rect.size;

        Vector2 worldPosition = target.position;
        Vector2 viewportPosition = Camera.main.ScreenToViewportPoint(worldPosition);
        Vector2 targetCenter = new Vector2(viewportPosition.x, viewportPosition.y);

        Vector2 size = target.rect.size;
        Vector2 targetSize = size / canvasSize;

        Vector2 startCenter = material.GetVector("_HoleCenter");
        Vector2 startSize = material.GetVector("_HoleSize");

        moveTween?.Kill();
        sizeTween?.Kill();

        moveTween = DOTween.To(() => startCenter, 
            val => material.SetVector("_HoleCenter", val), 
            targetCenter, 
            animationDuration);

        sizeTween = DOTween.To(() => startSize, 
            val => material.SetVector("_HoleSize", val), 
            targetSize, 
            animationDuration);
    }

    public void HideHole()
    {
        moveTween?.Kill();
        sizeTween?.Kill();

        spotlightMaterial.SetVector("_HoleSize", Vector2.zero);
    }
}
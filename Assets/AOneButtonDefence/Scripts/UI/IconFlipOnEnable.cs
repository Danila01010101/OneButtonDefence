using UnityEngine;
using DG.Tweening;

public class IconFlipOnEnable : MonoBehaviour
{
    [SerializeField] private RectTransform iconTransform; 
    [SerializeField] private float duration = 0.4f; 
    [SerializeField] private float overshoot = 1.1f;
    [SerializeField] private bool flipY;

    private void OnEnable()
    {
        if (iconTransform == null)
            iconTransform = GetComponent<RectTransform>();

        DOTween.Kill(iconTransform);

        iconTransform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();
        if (flipY)
        {
            seq.Append(iconTransform.DOScaleY(0f, duration * 0.5f).SetEase(Ease.InQuad));
            seq.Append(iconTransform.DOScaleY(overshoot, duration * 0.3f).SetEase(Ease.OutBack));
            seq.Append(iconTransform.DOScaleY(1f, duration * 0.2f).SetEase(Ease.InOutQuad));
        }
        else
        {
            seq.Append(iconTransform.DOScaleX(0f, duration * 0.5f).SetEase(Ease.InQuad));
            seq.Append(iconTransform.DOScaleX(overshoot, duration * 0.3f).SetEase(Ease.OutBack));
            seq.Append(iconTransform.DOScaleX(1f, duration * 0.2f).SetEase(Ease.InOutQuad));
        }
        seq.SetTarget(iconTransform);
    }
}
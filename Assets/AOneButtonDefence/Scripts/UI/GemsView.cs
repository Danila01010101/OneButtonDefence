using DG.Tweening;
using TMPro;
using UnityEngine;

public class GemsView : ResourceValueView
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector3 bounceValue;
    [SerializeField] private float duration;

    private Vector3 originalTextScale;

    public static GemsView Instance { get; private set; }
    public RectTransform GemsTextTransform => valueText.rectTransform;
    public Canvas Canvas => canvas;

    private void Awake()
    {
        originalTextScale = valueText.transform.localScale;
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public override void UpdateValue()
    {
        base.UpdateValue();
        AnimateText(bounceValue, duration);
    }

    public override void UpdateTurnIncomeValue(string newValue, bool isPositive) { }

    private void AnimateText(Vector3 bounceValue, float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GemsTextTransform.DOScale(originalTextScale + bounceValue, duration / 2)
            .SetEase(Ease.OutQuad));
        sequence.Append(GemsTextTransform.DOScale(originalTextScale, duration / 2)
            .SetEase(Ease.InQuad));
    }
}
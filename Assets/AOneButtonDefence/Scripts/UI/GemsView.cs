using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GemsView : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private Vector3 bounceValue;
    [SerializeField] private float duration;

    private Vector3 originalTextScale;

    public static GemsView Instance { get; private set; }
    public RectTransform GemsTextTransform => gemsText.rectTransform;
    public Canvas Canvas => canvas;

    private void Awake()
    {
        originalTextScale = gemsText.transform.localScale;
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        UpdateGemsValue(ResourcesCounter.Instance.Data.GemsAmount);
    }

    private void UpdateGemsValue(int value)
    {
        AnimateText(bounceValue, duration);
        gemsText.text = value.ToString();
    }

    private void AnimateText(Vector3 bounceValue, float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GemsTextTransform.DOScale(originalTextScale + bounceValue, duration / 2)
            .SetEase(Ease.OutQuad));
        sequence.Append(GemsTextTransform.DOScale(originalTextScale, duration / 2)
            .SetEase(Ease.InQuad));
    }


    private void OnEnable()
    {
        ResourcesCounter.Instance.Data.GemsAmountChanged += UpdateGemsValue;
    }

    private void OnDisable()
    {
        ResourcesCounter.Instance.Data.GemsAmountChanged -= UpdateGemsValue;
    }
}
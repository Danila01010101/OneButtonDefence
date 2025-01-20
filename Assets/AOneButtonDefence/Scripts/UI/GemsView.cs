using System;
using TMPro;
using UnityEngine;

public class GemsView : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI gemsText;

    public static GemsView Instance { get; private set; }
    public RectTransform GemsTextTransform => gemsText.rectTransform;
    public Canvas Canvas => canvas;

    private void Awake()
    {
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

    private void UpdateGemsValue(int value) => gemsText.text = value.ToString();

    private void OnEnable()
    {
        ResourcesCounter.Instance.Data.GemsAmountChanged += UpdateGemsValue;
    }

    private void OnDisable()
    {
        ResourcesCounter.Instance.Data.GemsAmountChanged -= UpdateGemsValue;
    }
}
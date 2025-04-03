using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{
    [Header("Основные элементы UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image buildingIcon;
    [SerializeField] private TextMeshProUGUI loreText;

    [Header("Настройки для генерации строк")]
    [SerializeField] private Transform perBuildingContainer;
    [SerializeField] private Transform perRoundContainer;
    [SerializeField] private BuildingResourceInfoView resourceInfoPrefab;

    [Header("Настройки позиционирования")]
    [SerializeField] private Vector3 startMainWindowposition;
    [SerializeField] private float screenSizeIncreaseValue;
    [SerializeField] private float distancePerString;

    private readonly List<BuildingResourceInfoView> activeResourceRows = new List<BuildingResourceInfoView>();
    private RectTransform panelRectTransform;

    private void Awake()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(BasicBuildingData buildingData)
    {
        ClearData();
        nameText.text = buildingData.UpgradeType.ToString();
        buildingIcon.sprite = buildingData.Icon;
        
        int maxRows = Mathf.Max(buildingData.buildResourceChange.Length, buildingData.resourcePerTurnChange.Length);
        PopulateResourceList(buildingData.buildResourceChange, perBuildingContainer);
        PopulateResourceList(buildingData.resourcePerTurnChange, perRoundContainer);
        loreText.text = buildingData.BuildingLore;

        AdjustWindowSize(maxRows);
    }

    private void PopulateResourceList(BasicBuildingData.ResourceChangeInfo[] resources, Transform container)
    {
        float offsetY = 0;
        foreach (var resourceInfo in resources)
        {
            var newRow = Instantiate(resourceInfoPrefab, container);
            activeResourceRows.Add(newRow);

            newRow.SetResourceInfo(resourceInfo);
            var rectTransform = newRow.GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(0, offsetY);
            offsetY += distancePerString;
        }
    }

    private void AdjustWindowSize(int maxRows)
    {
        float sizeIncrease = screenSizeIncreaseValue * maxRows;
        
        panelRectTransform.sizeDelta += new Vector2(0, sizeIncrease);
        panelRectTransform.anchoredPosition = startMainWindowposition + new Vector3(0, sizeIncrease / 2, 0);
    }

    private void ClearData()
    {
        nameText.text = "";
        buildingIcon.sprite = null;
        loreText.text = "";

        foreach (var row in activeResourceRows)
        {
            Destroy(row.gameObject);
        }
        activeResourceRows.Clear();

        panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 0);
        panelRectTransform.anchoredPosition = startMainWindowposition;
    }
}

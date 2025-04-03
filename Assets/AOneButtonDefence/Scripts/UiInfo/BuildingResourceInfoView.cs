using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingResourceInfoView : MonoBehaviour
{
    [Header("UI Элементы")]
    public Image resourceIcon;
    public TextMeshProUGUI resourceAmountText;
    public TextMeshProUGUI resourceDescriptionText;

    /// <summary>
    /// Устанавливает информацию о ресурсе в элементы UI.
    /// </summary>
    public void SetResourceInfo(BasicBuildingData.ResourceChangeInfo resourceInfo)
    {
        if (resourceInfo.ResourceDescription == null || resourceInfo.ResourceAmount.Resource == null)
        {
            Debug.LogWarning("Попытка установить пустые данные ресурса!");
            return;
        }

        resourceIcon.sprite = resourceInfo.ResourceAmount.Resource.Icon;
        resourceAmountText.text = $"{(resourceInfo.ResourceAmount.Amount >= 0 ? "+" : "")}{resourceInfo.ResourceAmount.Amount}";
        resourceDescriptionText.text = resourceInfo.ResourceDescription;

        gameObject.SetActive(true);
    }
}
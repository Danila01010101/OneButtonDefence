using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StatisticView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI foodValueText;
    [SerializeField] private TextMeshProUGUI spiritValueText;
    [SerializeField] private TextMeshProUGUI strengthValueText;
    [SerializeField] private TextMeshProUGUI materialsValueText;
    [SerializeField] private TextMeshProUGUI gemsText;
    
    private void Start()
    {
        SubscribeForValueChanging();
    }

    private void UpdateFoodValue(int newValue) => foodValueText.text = newValue.ToString();

    private void UpdateSpiritValue(int newValue) => spiritValueText.text = newValue.ToString();

    private void UpdateStrengthValue(int newValue) => strengthValueText.text = newValue.ToString();

    private void UpdateMaterialsValue(int newValue) => materialsValueText.text = newValue.ToString();

    private void UpdateGemsValue(int newValue)
    {
        GemsView.Instance.PlayUpdateAnimation();
        gemsText.text = newValue.ToString();
    }

    private void DetectMaterialsChange(ResourcesCounter.ResourcesData data)
    {
        UpdateFoodValue(data.FoodAmount);
        UpdateSpiritValue(data.SurvivorSpirit);
        UpdateStrengthValue(data.Warriors);
        UpdateMaterialsValue(data.Materials);
        UpdateGemsValue(data.GemsAmount);
    }

    private void SubscribeForValueChanging()
    {
        ResourcesCounter.ResourcesAmountChanged += DetectMaterialsChange;
    }

    private void UnsubscribeForValueChanging()
    {
        ResourcesCounter.ResourcesAmountChanged -= DetectMaterialsChange;
    }

    private void OnDestroy()
    {
        UnsubscribeForValueChanging();
    }
}
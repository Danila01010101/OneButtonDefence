using TMPro;
using UnityEngine;

public class StatisticsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI foodValueText;
    [SerializeField] private TextMeshProUGUI spiritValueText;
    [SerializeField] private TextMeshProUGUI strenghtValueText;
    [SerializeField] private TextMeshProUGUI materialsValueText;

    private void UpdateFoodValue(int newValue) => foodValueText.text = newValue.ToString();

    private void UpdateSpiritValue(int newValue) => spiritValueText.text = newValue.ToString();

    private void UpdateStrenghtValue(int newValue) => strenghtValueText.text = newValue.ToString();

    private void UpdateMaterialsValue(int newValue) => materialsValueText.text = newValue.ToString();

    private void SubscribeForValueChanging()
    {
        ResourcesCounter.Instance.Data.FoodAmountChanged += UpdateFoodValue;
        ResourcesCounter.Instance.Data.SpiritAmountChanged += UpdateSpiritValue;
        ResourcesCounter.Instance.Data.WarriorsAmountChanged += UpdateStrenghtValue;
        ResourcesCounter.Instance.Data.MaterialsAmountChanged += UpdateMaterialsValue;
    }

    private void UnsubscribeForValueChanging()
    {
        ResourcesCounter.Instance.Data.FoodAmountChanged -= UpdateFoodValue;
        ResourcesCounter.Instance.Data.SpiritAmountChanged -= UpdateSpiritValue;
        ResourcesCounter.Instance.Data.WarriorsAmountChanged -= UpdateStrenghtValue;
        ResourcesCounter.Instance.Data.MaterialsAmountChanged -= UpdateMaterialsValue;
    }

    private void OnEnable()
    {
        SubscribeForValueChanging();
    }

    private void OnDisable()
    {
        UnsubscribeForValueChanging();
    }
}
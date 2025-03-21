using TMPro;
using UnityEngine;

	public class IncomeView : MonoBehaviour
	{
		[SerializeField] private Color positiveColor = Color.green;
		[SerializeField] private Color negativeColor = Color.red;
		[SerializeField] private TextMeshProUGUI foodIncomeText;
		[SerializeField] private TextMeshProUGUI spiritIncomeText;
		[SerializeField] private TextMeshProUGUI strengthIncomeText;
		[SerializeField] private TextMeshProUGUI materialsIncomeText;

		private void UpdateFoodIncome(string newValue, bool isPositive)
		{
			foodIncomeText.text = newValue;
			foodIncomeText.color = isPositive ? positiveColor : negativeColor;
		}

		private void UpdateSpiritIncome(string newValue, bool isPositive)
		{
			spiritIncomeText.text = newValue;
			spiritIncomeText.color = isPositive ? positiveColor : negativeColor;
		}

		private void UpdateStrengthIncome(string newValue, bool isPositive)
		{
			strengthIncomeText.text = newValue;
			strengthIncomeText.color = isPositive ? positiveColor : negativeColor;
		}

		private void UpdateMaterialsIncome(string newValue, bool isPositive)
		{
			materialsIncomeText.text = newValue;
			materialsIncomeText.color = isPositive ? positiveColor : negativeColor;
		}

		private void OnEnable()
		{
			IncomeDifferenceNotifier.FoodIncomeUpdated += UpdateFoodIncome;
			IncomeDifferenceNotifier.SpiritIncomeUpdated += UpdateSpiritIncome;
			IncomeDifferenceNotifier.MaterialsIncomeUpdated += UpdateMaterialsIncome;
			IncomeDifferenceNotifier.WarriorsIncomeUpdated += UpdateStrengthIncome;
		}

		private void OnDisable()
		{
			IncomeDifferenceNotifier.FoodIncomeUpdated += UpdateFoodIncome;
			IncomeDifferenceNotifier.SpiritIncomeUpdated -= UpdateSpiritIncome;
			IncomeDifferenceNotifier.MaterialsIncomeUpdated -= UpdateMaterialsIncome;
			IncomeDifferenceNotifier.WarriorsIncomeUpdated -= UpdateStrengthIncome;
		}
	}
public class FoodValueView : ResourceValueView
{

    protected override void Subscribe() => ResourcesCounter.MaterialsAmountChanged += UpdateValue;

    protected override void Unsubscribe() => ResourcesCounter.MaterialsAmountChanged -= UpdateValue;
}

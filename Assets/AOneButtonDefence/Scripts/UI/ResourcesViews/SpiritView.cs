public class SpiritView : ResourceValueView
{
    private void DetectMaterialsChange(ResourcesCounter.ResourcesData data) => UpdateValue(data.FoodAmount);

    protected override void Subscribe() => ResourcesCounter.ResourcesAmountChanged += DetectMaterialsChange;

    protected override void Unsubscribe() => ResourcesCounter.ResourcesAmountChanged -= DetectMaterialsChange;
}
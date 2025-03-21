public class MaterialsValue : ResourceValueView
{
    private void DetectMaterialsChange(ResourcesCounter.ResourcesData data) => UpdateValue(data.Materials);

    protected override void Subscribe() => ResourcesCounter.ResourcesAmountChanged += DetectMaterialsChange;

    protected override void Unsubscribe() => ResourcesCounter.ResourcesAmountChanged -= DetectMaterialsChange;
}


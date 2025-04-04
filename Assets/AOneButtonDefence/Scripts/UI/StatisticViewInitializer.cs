using UnityEngine;

public class StatisticViewInitializer : MonoBehaviour
{
    [SerializeField] private ResourceValueView foodView;
    [SerializeField] private ResourceValueView spiritView;
    [SerializeField] private ResourceValueView strengthView;
    [SerializeField] private ResourceValueView materialsView;
    [SerializeField] private ResourceValueView gemsView;

    public void Initialize(ResourceData.ResourceType foodResource,  ResourceData.ResourceType warriorResource,  ResourceData.ResourceType spiritResource,  ResourceData.ResourceType fabricResource, ResourceData.ResourceType gemsResource)
    {
        foodView.Initialize(foodResource, IncomeDifferenceTextConverter.FoodIncomeUpdated);
        spiritView.Initialize(spiritResource, IncomeDifferenceTextConverter.SpiritIncomeUpdated);
        strengthView.Initialize(warriorResource, IncomeDifferenceTextConverter.WarriorsIncomeUpdated);
        materialsView.Initialize(fabricResource, IncomeDifferenceTextConverter.MaterialsIncomeUpdated);
        gemsView.Initialize(gemsResource);
    }
}
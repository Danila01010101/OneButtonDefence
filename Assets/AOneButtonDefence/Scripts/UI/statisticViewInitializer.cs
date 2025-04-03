using UnityEngine;

public class statisticViewInitializer : MonoBehaviour
{
    [SerializeField] private ResourceValueView foodView;
    [SerializeField] private ResourceValueView spiritView;
    [SerializeField] private ResourceValueView strengthView;
    [SerializeField] private ResourceValueView materialsView;
    [SerializeField] private ResourceValueView gemsView;

    public void Initialize(ResourceData.ResourceType foodResource,  ResourceData.ResourceType warriorResource,  ResourceData.ResourceType spiritResource,  ResourceData.ResourceType fabricResource, ResourceData.ResourceType gemsResource)
    {
        foodView.Initialize(foodResource);
        spiritView.Initialize(spiritResource);
        strengthView.Initialize(warriorResource);
        materialsView.Initialize(fabricResource);
        gemsView.Initialize(gemsResource);
    }
}
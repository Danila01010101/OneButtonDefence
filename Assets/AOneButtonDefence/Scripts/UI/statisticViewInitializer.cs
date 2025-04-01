using UnityEngine;

public class statisticViewInitializer : MonoBehaviour
{
    [SerializeField] private ResourceValueView foodView;
    [SerializeField] private ResourceValueView spiritView;
    [SerializeField] private ResourceValueView strengthView;
    [SerializeField] private ResourceValueView materialsView;
    [SerializeField] private ResourceValueView gemsView;

    public void Initialize(ResourceData foodResource,  ResourceData warriorResource,  ResourceData spiritResource,  ResourceData fabricResource, ResourceData gemsResource)
    {
        foodView.Initialize(foodResource);
        spiritView.Initialize(spiritResource);
        strengthView.Initialize(warriorResource);
        materialsView.Initialize(fabricResource);
        gemsView.Initialize(gemsResource);
    }
}
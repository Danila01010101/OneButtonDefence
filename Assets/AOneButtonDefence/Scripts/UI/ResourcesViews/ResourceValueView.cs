using TMPro;
using UnityEngine;

public abstract class ResourceValueView : MonoBehaviour, IResourceView
{
    [SerializeField] protected TextMeshProUGUI valueText;
    [SerializeField] protected TextMeshProUGUI turnIncomeDifferenceText;
    
    private ResourceData.ResourceType resourceType;

    public void Initialize(ResourceData.ResourceType resourceType)
    {
        this.resourceType = resourceType;
        SubscribeForValueChanging();
    }
    
    public virtual void UpdateValue()
    {
        valueText.text = GameResourcesCounter.GetResourceAmount(resourceType).ToString();
    }
    
    public virtual void UpdateTurnIncomeValue(string newValue, bool isPositive) 
    {
        turnIncomeDifferenceText.color = isPositive ? Color.green : Color.red;
        turnIncomeDifferenceText.text = newValue;
    }

    private void SubscribeForValueChanging()
    {
        UpgradeState.UpgradeStateStarted += UpdateValue;
    }

    private void UnsubscribeForValueChanging()
    {
        UpgradeState.UpgradeStateStarted -= UpdateValue;
    }

    private void OnDestroy()
    {
        UnsubscribeForValueChanging();
    }
}
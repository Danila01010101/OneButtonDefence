using System;
using TMPro;
using UnityEngine;

public class ResourceValueView : MonoBehaviour, IResourceView
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

    public virtual void UpdateTurnIncomeValue(ResourceData.ResourceType type, string newValue, bool isPositive) 
    {
        if (turnIncomeDifferenceText != null && type != resourceType)
            return;
        
        turnIncomeDifferenceText.color = isPositive ? Color.green : Color.red;
        turnIncomeDifferenceText.text = newValue;
    }

    private void SubscribeForValueChanging()
    {
        GameResourcesCounter.ResourceAdded += UpdateValue;
        IncomeDifferenceTextConverter.ResourceIncomeChanged += UpdateTurnIncomeValue;
    }

    private void UnsubscribeForValueChanging()
    {
        GameResourcesCounter.ResourceAdded -= UpdateValue;
        IncomeDifferenceTextConverter.ResourceIncomeChanged -= UpdateTurnIncomeValue;
    }

    private void OnDestroy()
    {
        UnsubscribeForValueChanging();
    }
}
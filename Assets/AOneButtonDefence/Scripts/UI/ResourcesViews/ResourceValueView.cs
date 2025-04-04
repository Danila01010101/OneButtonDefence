using System;
using TMPro;
using UnityEngine;

public class ResourceValueView : MonoBehaviour, IResourceView
{
    [SerializeField] protected TextMeshProUGUI valueText;
    [SerializeField] protected TextMeshProUGUI turnIncomeDifferenceText;

    private Action<string, bool> incomeUpdated;
    private ResourceData.ResourceType resourceType;

    public void Initialize(ResourceData.ResourceType resourceType, Action<string, bool> incomeUpdated = null)
    {
        this.resourceType = resourceType;
        this.incomeUpdated = incomeUpdated;
        SubscribeForValueChanging();
    }

    public virtual void UpdateValue()
    {
        valueText.text = GameResourcesCounter.GetResourceAmount(resourceType).ToString();
    }
    
    public virtual void UpdateTurnIncomeValue(string newValue, bool isPositive) 
    {
        if (turnIncomeDifferenceText == null)
            return;
        
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
        
        if (incomeUpdated != null)
            incomeUpdated -= UpdateTurnIncomeValue;
    }

    private void OnDestroy()
    {
        UnsubscribeForValueChanging();
    }
}
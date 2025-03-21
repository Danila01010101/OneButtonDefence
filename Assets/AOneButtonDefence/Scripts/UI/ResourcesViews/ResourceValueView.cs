using TMPro;
using UnityEngine;

public abstract class ResourceValueView : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI valueText;
    [SerializeField] protected TextMeshProUGUI turnIncomeDifferenceText;

    protected abstract void Subscribe();
    protected abstract void Unsubscribe();
    
    protected void UpdateValue(int newValue)
    {
        valueText.text = newValue.ToString();
    }
    
    protected void UpdateTurnIncomeValue(string newValue) 
    {
        turnIncomeDifferenceText.text = newValue;
    }

    private void OnDestroy() => Unsubscribe();
}
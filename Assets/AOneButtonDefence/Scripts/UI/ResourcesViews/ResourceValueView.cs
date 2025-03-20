using TMPro;
using UnityEngine;

public abstract class ResourceValueView : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI valueText;

    protected abstract void Subscribe();
    protected abstract void Unsubscribe();
    
    protected void UpdateValue(int newValue)
    {
        valueText.text = newValue.ToString();
    }

    private void OnDestroy() => Unsubscribe();
}
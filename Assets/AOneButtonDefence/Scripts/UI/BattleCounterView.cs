using TMPro;
using UnityEngine;

public class BattleCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winsValueText;

    private void Start()
    {
        SubscribeForValueChanging();
    }

    private void UpdateWinsValue(int newValue) => winsValueText.text = newValue.ToString();

    private void SubscribeForValueChanging()
    {
        WaveCounter.Instance.OnWaveChanged += UpdateWinsValue;
    }

    private void UnsubscribeForValueChanging()
    {
        WaveCounter.Instance.OnWaveChanged -= UpdateWinsValue;
    }

    private void OnDestroy()
    {
        UnsubscribeForValueChanging();
    }
}
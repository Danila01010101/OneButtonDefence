using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeResourceLabel : MonoBehaviour
{
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI resourceText;

    public void Set(StartResourceAmount info)
    {
        resourceIcon.sprite = info.Resource.Icon;
        
        StringBuilder sb = new StringBuilder();
        sb.Append("Добавлено ");
        sb.Append(info.Amount > 0 ? " + " : " ");
        sb.Append(info.Amount);
        sb.Append(" ресурса ");
        sb.Append(info.Resource.Name);
        resourceText.text = sb.ToString();
    }
}

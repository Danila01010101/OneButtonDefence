
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInfoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI LoreText;
    public string Lore;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Мышка вошла!");
        LoreText.text = Lore;    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LoreText.text = "";
    }
}

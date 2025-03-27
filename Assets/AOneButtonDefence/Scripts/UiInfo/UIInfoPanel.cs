using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI Name;

    public List<TextMeshProUGUI> perBuildingText; 
    public List<Image> perBuildingSprites;

    public List<TextMeshProUGUI> perRoundText;
    public List<Image> perRoundSprites;

    public TextMeshProUGUI Lore;

    public void Initializator(InfoPanelScriptableObject panelData)
    {
        ClearningData();
        Name.text = panelData.Name();
        for (int i = 0; i < panelData.PerBuilding().Count; i++)
        {
            perBuildingText[i].gameObject.SetActive(true);
            perBuildingText[i].gameObject.SetActive(true);
            perBuildingText[i].text = panelData.PerBuilding()[i].Text;
            if (panelData.PerBuilding()[i].Image != null)
            {
                perBuildingSprites[i].sprite = panelData.PerBuilding()[i].Image;
                perBuildingSprites[i].gameObject.SetActive(true);
            }
            else
            {
                perBuildingSprites[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < panelData.PerRound().Count; i++)
        {
            perRoundText[i].gameObject.SetActive(true);
            perRoundSprites[i].gameObject.SetActive(true);
            perRoundText[i].text = panelData.PerRound()[i].Text;
            if (panelData.PerRound()[i].Image != null)
            {
                perRoundSprites[i].sprite = panelData.PerRound()[i].Image;
                perRoundSprites[i].gameObject.SetActive(true);
            }
            else
            {
                perRoundSprites[i].gameObject.SetActive(false);
            }
        }
        Lore.text = panelData.Lore();
    }
    private void ClearningData()
    {
        Name.text = "";
        for (int i = 0; i < perBuildingText.Count; i++)
        {
            perBuildingText[i].gameObject.SetActive(false);
            perBuildingSprites[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < perRoundText.Count; i++) 
        {
            perRoundText[i].gameObject.SetActive(false);
            perRoundSprites[i].gameObject.SetActive(false);
        }
        Lore.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New InfoPanelData", menuName = "ScriptableObjects/new InfoPanel Data", order = 51)]
public class InfoPanelScriptableObject : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private List<PanelSection> perBuilding;
    [SerializeField] private List<PanelSection> perRound;
    [SerializeField] private string lore;
    [SerializeField] private Image image;
    [SerializeField] private GameObject prefab;

    public string Name()
    {
        return name;
    }
    public List<PanelSection> PerBuilding()
    {
        return perBuilding;
    }
    public List<PanelSection> PerRound()
    {
        return perRound;
    }
    public string Lore()
    {
        return lore;
    }
    public Image Image()
    {
        return image;
    }
}

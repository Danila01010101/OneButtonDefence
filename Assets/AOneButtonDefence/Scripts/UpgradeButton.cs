using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    public static Action<Upgrades, Upgrades> UpgradeTypesChoosen;
    public static Action UpgradesChoosen;

    public enum Upgrades { Farm = 0, SpiritBuilding = 1, MilitaryCamp = 2, ResourcesCenter = 3 }

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void UpgradeChosenPart(Upgrades firstPart, Upgrades secondPart)
    {
        UpgradesChoosen?.Invoke();
        UpgradeTypesChoosen(firstPart, secondPart);
    }

    public void Activate() => button.interactable = true;

    public void Deactivate() => button.interactable = false;
}

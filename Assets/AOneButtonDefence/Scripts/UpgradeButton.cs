using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    public static Action<Building.BuildingType> Upgrade;
    public static Action UpgradeChoosen;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void UbgraidChoisenPart(Building.BuildingType firstPart, Building.BuildingType secondPart)
    {
        Upgrade(firstPart);
        Upgrade(secondPart);
        UpgradeChoosen?.Invoke();
    }

    public void Activate() => button.interactable = true;

    public void Deactivate() => button.interactable = false;
}

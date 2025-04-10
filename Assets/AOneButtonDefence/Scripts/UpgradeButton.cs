using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    public static Action<BasicBuildingData.Upgrades, BasicBuildingData.Upgrades> UpgradeTypesChoosen;
    public static Action UpgradesChoosen;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void UpgradeChosenPart(BasicBuildingData.Upgrades firstPart, BasicBuildingData.Upgrades secondPart)
    {
        UpgradesChoosen?.Invoke();
        UpgradeTypesChoosen(firstPart, secondPart);
    }

    public void Activate() => button.interactable = true;

    public void Deactivate() => button.interactable = false;
}

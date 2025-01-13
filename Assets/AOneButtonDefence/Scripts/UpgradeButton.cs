using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    public static Action<Upgrades> Upgrade;
    public static Action UpgradeChoosen;

    public enum Upgrades { Farm = 0, SpiritBuilding = 1, MilitaryCamp = 2, ResourcesCenter = 3 }

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void UpgraidChoisenPart(Upgrades firstPart, Upgrades secondPart)
    {
        UpgradeChoosen?.Invoke();
        Upgrade(firstPart);
        Upgrade(secondPart);
    }

    public void Activate() => button.interactable = true;

    public void Deactivate() => button.interactable = false;
}

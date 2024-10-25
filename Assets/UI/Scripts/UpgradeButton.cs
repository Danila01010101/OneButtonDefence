using System;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public static Action<Upgrades> Upgrade;
    public static Action UpgradeChoosen;

    public enum Upgrades { Farm = 0, SpiritBuilding = 1, MilitaryCamp = 2, ResourcesCenter = 3 }

    public void UbgraidChoisenPart(Upgrades firstPart, Upgrades secondPart)
    {
        UpgradeChoosen?.Invoke();
        Upgrade(firstPart);
        Upgrade(secondPart);
    }
}

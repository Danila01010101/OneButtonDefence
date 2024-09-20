using System;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public static Action<Upgrades> Upgrade;

    public enum Upgrades { Farm = 0, SpiritBuilding = 1, MilitaryCamp = 2, ResourcesCenter = 3 }

    public void UbgraidChoisenPart(Upgrades firstPart, Upgrades secondPart)
    {
        Debug.Log(firstPart);
        Debug.Log(secondPart);
        Upgrade(firstPart);
        Upgrade(secondPart);
    }
}

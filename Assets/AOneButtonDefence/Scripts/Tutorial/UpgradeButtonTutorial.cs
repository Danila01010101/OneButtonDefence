using System;
using UnityEngine;

public class UpgradeButtonTutorial : TutorialObject
{
    [SerializeField] private UpgradeButton upgradeButton;
    
    private void Awake()
    {
        upgradeButton.Activated += delegate { isActivated = true; };
    }

    private void OnDestroy()
    {
        upgradeButton.Activated -= delegate { isActivated = true; };
    }
}
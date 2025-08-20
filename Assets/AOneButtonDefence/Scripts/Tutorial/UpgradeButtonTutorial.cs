using System;
using UnityEngine;

public class UpgradeButtonTutorial : TutorialObject
{
    [SerializeField] private UpgradeButton upgradeButton;

    private Action handler;
    
    private void Awake()
    {
        handler = delegate { isActivated = true; };
        upgradeButton.Activated += handler;
    }

    private void OnDestroy()
    {
        upgradeButton.Activated -= handler;
    }
}
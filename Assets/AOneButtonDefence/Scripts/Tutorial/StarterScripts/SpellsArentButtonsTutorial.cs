using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpellsArentButtonsTutorial : TutorialObject
{
    [SerializeField] private Button spellButtonComponent;
    [SerializeField] private Image spellBackground;
    
    private bool isShown = false;
    
    private UnityAction handler;

    private void Awake()
    {
        handler = delegate
        {
            isActivated = true;
            TimeManager.SetTimeScale(0.15f);
            TutorialMessage.TutorialWindowDestroyed += ResetGameTime;
        };

        spellButtonComponent.onClick.AddListener(handler);
        GameBattleState.BattleWon += DiactivateIfNotShown;
    }

    private void DiactivateIfNotShown()
    {
        if (isShown == false)
        {
            Deactivate();
            GameBattleState.BattleWon -= DiactivateIfNotShown;
        }
    }

    private void ResetGameTime()
    {
        if (IsActivated)
        {
            Deactivate();
            TutorialMessage.TutorialWindowDestroyed -= ResetGameTime;
            isShown = true;
        }
    }

    private void Deactivate()
    {
        spellBackground.raycastTarget = false;
        Color newColor = spellBackground.color;
        newColor.a = 0.5f;
        
        if (spellBackground!= null)
            spellBackground.color = newColor;
        
        if (spellButtonComponent!= null)
            spellButtonComponent.onClick.RemoveListener(handler);
        
        TimeManager.SetTimeScale(1f);
        Destroy(spellButtonComponent);
        Destroy(this);
    }
}
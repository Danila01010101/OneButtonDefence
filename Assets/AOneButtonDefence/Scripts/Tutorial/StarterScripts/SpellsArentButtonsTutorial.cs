using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpellsArentButtonsTutorial : TutorialObject
{
    [SerializeField] private Button spellButtonComponent;
    [SerializeField] private Image spellBackground;
    
    private UnityAction handler;

    private void Awake()
    {
        handler = delegate
        {
            isActivated = true;
            TimeManager.SetTimeScale(0.15f);
        };

        spellButtonComponent.onClick.AddListener(handler);
        TutorialMessage.TutorialWindowDestroyed += ResetGameTime;
    }

    private void ResetGameTime()
    {
        if (IsActivated)
        {
            spellBackground.raycastTarget = false;
            Color newColor = spellBackground.color;
            newColor.a = 0.5f;
            spellBackground.color = newColor;
            TimeManager.SetTimeScale(1f);
            spellButtonComponent.onClick.RemoveListener(handler);
            TutorialMessage.TutorialWindowDestroyed -= ResetGameTime;
            Destroy(spellButtonComponent);
            Destroy(this);
        }
    }
}
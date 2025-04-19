using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpellCanvas : MonoBehaviour
{
    [SerializeField] private Image currentSpell;
    [SerializeField] private Image currentSpellBackground;
    [SerializeField] private Image nextSpell;
    [SerializeField] private Image nextSpellIcon;
    
    public void ChangeUI(Sprite currentSpell, Sprite nextSpell, Sprite currentSpellBackground, Sprite nextSpellIcon, float duration)
    {
        this.currentSpell.sprite = currentSpell;
        this.currentSpellBackground.sprite = currentSpellBackground;
        this.nextSpellIcon.sprite = nextSpellIcon;
        this.nextSpell.sprite = nextSpell;
        this.currentSpell.fillAmount = 0f;
        this.currentSpell.DOFillAmount(1f, duration).SetEase(Ease.Linear);
    }
}

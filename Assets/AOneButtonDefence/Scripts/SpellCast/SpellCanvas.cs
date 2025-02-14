using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public class SpellCanvas : MonoBehaviour
{
    [FormerlySerializedAs("currrentSpell")] [FormerlySerializedAs("CurrentSpell")] [SerializeField] private Image currentSpell;
    [FormerlySerializedAs("NextSpell")] [SerializeField] private Image nextSpell;
    
    public void ChangeUI(Sprite currentSpell, Sprite nextSpell, float duration)
    {
        this.currentSpell.sprite = currentSpell;
        this.nextSpell.sprite = nextSpell;
        this.currentSpell.fillAmount = 0f;
        this.currentSpell.DOFillAmount(1f, duration).SetEase(Ease.Linear);
    }
}

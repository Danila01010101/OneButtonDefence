using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCanvas : MonoBehaviour
{
    public Image CurrentSpell;
    public Image NextSpell;
    public Button CurrentSpellButton;
    
    public void ChangeUI(Sprite currentSpell, Sprite nextSpell)
    {
        CurrentSpell.sprite = currentSpell;
        NextSpell.sprite = nextSpell;
    }
}

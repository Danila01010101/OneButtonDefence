using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New SpellData", menuName = "ScriptableObjects/new Spell Data", order = 58)]
public class SpellData : ScriptableObject
{
    [Header("Statistics")]
    public float Time;
    public int Damage;
    [Header("BaseGraphic")]
    public GameObject BaseMagicCircle;
    public Sprite IconForUI;
    [Header("Graphic")]
    public Color MainColor = new Color(0f, 0f, 0f, 1f);
    public Color SecondaryColor = new Color(0f, 0f, 0f, 1f);
    public Sprite EffectParticles;
    [Header("Fine tuning")]
    [ContextMenuItem("ResetColor", "ResetColor")]
    public bool FineTuning;
    
    [ConditionalHide("FineTuning")]
    public Color CircleColor = new Color(0f, 0f, 0f, 1f);
    [ConditionalHide("FineTuning")]
    public Color SidesColor = new Color(0f, 0f, 0f, 1f);
    [ConditionalHide("FineTuning")]
    public Color LightColor = new Color(0f, 0f, 0f, 1f);
    [ConditionalHide("FineTuning")]
    public Color SparksColor = new Color(0f, 0f, 0f, 1f);
    [ConditionalHide("FineTuning")]
    public Color FlakesColor = new Color(0f, 0f, 0f, 1f);

    private void ResetColor()
    {
        CircleColor = MainColor;
        SidesColor = SecondaryColor;
        LightColor = MainColor;
        SparksColor = SecondaryColor;
        FlakesColor = MainColor;
    }
}

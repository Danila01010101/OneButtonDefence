using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New SpellData", menuName = "ScriptableObjects/new Spell Data", order = 58)]
public class SpellData : ScriptableObject
{
    public int Blyat{ get; private set; }
    [Header("Statistics")]
    [SerializeField] private float time;
    [SerializeField] private float timePerDamage;
    [SerializeField] private int damage;
    [Header("BaseGraphic")]
    [SerializeField] private GameObject baseMagicCircle;
    [SerializeField] private Sprite iconForUI;
    [SerializeField] private Sprite background;
    [SerializeField] private Sprite miniIcon;
    [Header("Graphic")]
    [SerializeField] private Color mainColor = new Color(0f, 0f, 0f, 1f);
    [SerializeField] private Color secondaryColor = new Color(0f, 0f, 0f, 1f);
    [SerializeField] private Texture2D effectParticles;
    [Header("Fine tuning")]

    [ContextMenuItem("ResetColor", "ResetColor")]
    [SerializeField] private bool fineTuning;

    [SerializeField] private Color circleColor = new Color(0f, 0f, 0f, 1f);
    [SerializeField] private Color sidesColor = new Color(0f, 0f, 0f, 1f);
    [SerializeField] private Color lightColor = new Color(0f, 0f, 0f, 1f);
    [SerializeField] private Color sparksColor = new Color(0f, 0f, 0f, 1f);
    [SerializeField] private Color flakesColor = new Color(0f, 0f, 0f, 1f);

    public float Time => time;
    public float TimePerDamage => timePerDamage;
    public int Damage => damage;
    public GameObject BaseMagicCircle => baseMagicCircle;
    public Sprite IconForUI => iconForUI;
    public Sprite Background => background;
    public Sprite MiniIcon => miniIcon;
    public Color MainColor => mainColor;
    public Color SecondaryColor => secondaryColor;
    public Texture2D EffectParticles => effectParticles;

    public bool FineTuning => fineTuning;
    public Color CircleColor => circleColor;
    public Color SidesColor => sidesColor;
    public Color LightColor => lightColor;
    public Color SparksColor => sparksColor;
    public Color FlakesColor => flakesColor;
    
    private void ResetColor()
    {
        circleColor = mainColor;
        sidesColor = secondaryColor;
        lightColor = mainColor;
        sparksColor = secondaryColor;
        flakesColor = mainColor;
    }
}

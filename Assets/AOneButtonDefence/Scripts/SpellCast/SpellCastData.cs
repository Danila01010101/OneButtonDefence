using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New SpellCastData", menuName = "ScriptableObjects/new SpellCast Data", order = 59)]
public class SpellCastData : ScriptableObject
{
    public LayerMask spellCastLayerSurface;
    public float reloadDuration = 3f;
    public List<SpellStorage> SpellStorage;
}

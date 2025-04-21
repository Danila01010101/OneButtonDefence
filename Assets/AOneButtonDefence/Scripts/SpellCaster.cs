using UnityEngine;

public static class SpellCaster
{
    public static void Cast(GameObject spellGameObject, SpellData spellData, Vector3 castPosition, LayerMask targetLayer)
    {
        GameObject spell = GameObject.Instantiate(spellGameObject, castPosition, Quaternion.identity);
        spell.GetComponent<Spell>().Initialize(spellData, targetLayer);
    }
}
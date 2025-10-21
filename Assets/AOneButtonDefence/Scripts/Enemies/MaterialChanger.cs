using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MaterialChanger
{
    private FightingUnit fightingUnit;

    public MaterialChanger(FightingUnit fightingUnit)
    {
        this.fightingUnit = fightingUnit;
    }

    public void ChangeMaterialColour(Renderer renderer, Color startcolor, Color endcolor, float duration, float startfordisinvis)
    {
        Material material = renderer.material;
        material.color = startcolor;
        fightingUnit.StartCoroutine(Fade(material, endcolor, duration, startfordisinvis));
    }

    private IEnumerator Fade(Material material, Color endcolor, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        material.DOColor(endcolor, duration);
        yield return new WaitForSeconds(duration);
    }
}
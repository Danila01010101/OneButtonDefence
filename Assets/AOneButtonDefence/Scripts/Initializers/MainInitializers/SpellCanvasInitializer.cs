using System.Collections;
using UnityEngine;

public class SpellCanvasInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private SpellCanvas spellCanvasPrefab;
    [SerializeField] private SpellCastData spellCastData;

    public static GameObject SpellCanvas { get; private set; }

    public IEnumerator Initialize()
    {
        var canvasInstance = Instantiate(spellCanvasPrefab);
        new SpellCast(InputInitializer.Input, canvasInstance, spellCastData);
        canvasInstance.gameObject.SetActive(false);

        SpellCanvas = canvasInstance.gameObject;

        yield return null;
    }
}
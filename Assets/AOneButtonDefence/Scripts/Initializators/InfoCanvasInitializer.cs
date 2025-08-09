using UnityEngine;
using System.Collections;

public class InfoCanvasInitializer : IGameInitializerStep
{
    private GameObject _prefab;
    private GameplayCanvas _canvas;

    public InfoCanvasInitializer(GameObject prefab, GameplayCanvas canvas)
    {
        _prefab = prefab;
        _canvas = canvas;
    }

    public IEnumerator Initialize()
    {
        var infoCanvasInstance = Object.Instantiate(_prefab);
        _canvas.DetectSettingsWindow(infoCanvasInstance);
        infoCanvasInstance.SetActive(false);
        yield break;
    }
}
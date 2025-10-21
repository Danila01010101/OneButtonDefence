using UnityEngine;
using System.Collections;

public class InfoCanvasInitializer : IGameInitializerStep
{
    private ClosableWindow _prefab;
    private GameplayCanvas _canvas;

    public InfoCanvasInitializer(ClosableWindow prefab, GameplayCanvas canvas)
    {
        _prefab = prefab;
        _canvas = canvas;
    }

    public IEnumerator Initialize()
    {
        var infoCanvasInstance = Object.Instantiate(_prefab);
        _canvas.DetectSettingsWindow(infoCanvasInstance);
        infoCanvasInstance.gameObject.SetActive(false);
        yield break;
    }
}
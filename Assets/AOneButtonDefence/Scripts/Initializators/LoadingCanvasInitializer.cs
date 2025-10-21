using UnityEngine;
using System.Collections;

public class LoadingCanvasInitializer : IGameInitializerStep
{
    private Canvas _loadingCanvas;
    public Canvas Instance { get; private set; }

    public LoadingCanvasInitializer(Canvas prefab)
    {
        _loadingCanvas = prefab;
    }

    public IEnumerator Initialize()
    {
        Instance = Object.Instantiate(_loadingCanvas);
        yield break;
    }
}
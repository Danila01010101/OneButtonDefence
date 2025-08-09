using UnityEngine;
using System.Collections;

public class DebugCanvasInitializer : IGameInitializerStep
{
    private GameObject _prefab;
    private bool _isTestBuild;

    public DebugCanvasInitializer(GameObject prefab, bool isTestBuild)
    {
        _prefab = prefab;
        _isTestBuild = isTestBuild;
    }

    public IEnumerator Initialize()
    {
        var debugCanvasWindow = Object.Instantiate(_prefab);
        if (!_isTestBuild)
            debugCanvasWindow.SetActive(false);
        yield break;
    }
}
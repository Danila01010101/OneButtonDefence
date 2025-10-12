using UnityEngine;
using System.Collections;

public class DebugCanvasInitializer : IGameInitializerStep
{
    private GameObject _prefab;
    private GameResourcesCounter _counter;
    private ResourceData _gemsResource;
    private bool _isTestBuild;

    public DebugCanvasInitializer(GameObject prefab, bool isTestBuild, GameResourcesCounter counter, ResourceData gemsResource)
    {
        _prefab = prefab;
        _isTestBuild = isTestBuild;
        _gemsResource = gemsResource;
        _counter = counter;
    }

    public IEnumerator Initialize()
    {
        if (_isTestBuild)
            _counter.ChangeResourceAmount(new ResourceAmount(_gemsResource, 1000));
        
        var debugCanvasWindow = Object.Instantiate(_prefab);
        
        if (!_isTestBuild)
            debugCanvasWindow.SetActive(false);
        
        yield break;
    }
}
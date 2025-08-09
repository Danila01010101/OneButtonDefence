using UnityEngine;
using System.Collections;
using WrightAngle.Waypoint;

public class BattleCanvasInitializer : IGameInitializerStep
{
    private WaypointUIManager _prefab;
    private WaypointSettings _settings;
    private Camera _camera;

    public BattleCanvasInitializer(WaypointUIManager prefab, WaypointSettings settings, Camera camera)
    {
        _prefab = prefab;
        _settings = settings;
        _camera = camera;
    }

    public IEnumerator Initialize()
    {
        var canvasWindow = Object.Instantiate(_prefab);
        canvasWindow.Initializator(_camera, canvasWindow.GetComponent<RectTransform>(), _settings);
        yield break;
    }
}
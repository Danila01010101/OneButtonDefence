using UnityEngine;
using System.Collections;

public class CameraMovementInitializer : IGameInitializerStep
{
    private Cinemachine.CinemachineVirtualCamera _prefab;
    private Transform _parent;
    private IInput _input;
    private CameraData _cameraData;

    public CameraMovementInitializer(Cinemachine.CinemachineVirtualCamera prefab, Transform parent, IInput input, CameraData data)
    {
        _prefab = prefab;
        _parent = parent;
        _input = input;
        _cameraData = data;
    }

    public IEnumerator Initialize()
    {
        CameraMovement cameraMovement = Object.Instantiate(_prefab).gameObject.AddComponent<CameraMovement>();
        cameraMovement.transform.SetParent(_parent);
        cameraMovement.gameObject.name = "CameraMovement";
        cameraMovement.Initialize(_input, _cameraData);
        yield break;
    }
}
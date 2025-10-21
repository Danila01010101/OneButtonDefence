using UnityEngine;
using System.Collections;

public class DialogCameraInitializer : IGameInitializerStep
{
    private CameraData _cameraData;

    public DialogCameraInitializer(CameraData data)
    {
        _cameraData = data;
    }

    public IEnumerator Initialize()
    {
        var dialogCamera = Object.Instantiate(_cameraData.DialogCameraPrefab);
        dialogCamera.transform.position = _cameraData.DialogCameraPosition;
        dialogCamera.transform.eulerAngles = _cameraData.DialogCameraEulerAngles;
        yield break;
    }
}
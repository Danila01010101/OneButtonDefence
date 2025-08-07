using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;
    [SerializeField] private CameraData cameraData;

    public IEnumerator Initialize()
    {
        var dialogCamera = Instantiate(cameraData.DialogCameraPrefab);
        dialogCamera.transform.position = cameraData.DialogCameraPosition;
        dialogCamera.transform.eulerAngles = cameraData.DialogCameraEulerAngles;

        var cameraMovement = Instantiate(virtualCameraPrefab).gameObject.AddComponent<CameraMovement>();
        cameraMovement.Initialize(InputInitializer.Input, cameraData);
        cameraMovement.name = "CameraMovement";

        yield return null;
    }
}
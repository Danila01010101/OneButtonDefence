using Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "ScriptableObjects/New Camera Data")]
public class CameraData : ScriptableObject
{
    [field : SerializeField] public Vector3 StartCameraTargetPosition { get; private set; }
    [field : SerializeField] public CinemachineVirtualCamera DialogCameraPrefab { get; private set; }
    [field : SerializeField] public Vector3 DialogCameraPosition { get; private set; }
    [field : SerializeField] public Vector3 DialogCameraEulerAngles { get; private set; }
    [field : SerializeField] public float MinimumCameraDistance { get; private set; }
    [field : SerializeField] public float MaximumCameraDistance { get; private set; }
    [field : SerializeField] public float CameraMovementSpeed { get; private set; }
    [field : SerializeField] public float CameraRotateSpeed { get; private set; }
    [field : SerializeField] public float MaxRotationSpeed { get; private set; }
    [field : SerializeField] public float CameraRotationSmooth { get; private set; }
    [field : SerializeField] public float MinimumXAngle { get; private set; }
    [field : SerializeField] public float MaximumXAngle { get; private set; }
    [field : SerializeField] public float CameraZoomSpeed { get; private set; }
}
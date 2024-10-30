using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "ScriptableObjects/New Camera Data")]
public class CameraData : ScriptableObject
{
    [field : SerializeField] public Vector3 StartCameraPosition { get; private set; }
    [field : SerializeField] public float MinimumCameraHeight { get; private set; }
    [field : SerializeField] public float MaximumCameraHeight { get; private set; }
    [field : SerializeField] public float CameraMovementSpeed { get; private set; }
}
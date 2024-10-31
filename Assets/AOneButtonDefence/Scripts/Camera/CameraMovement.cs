using Cinemachine;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    private CameraData data;
    private Transform target;
    private CinemachineVirtualCamera virtualCamera;
    private IInput input;
    private Vector3 offset;
    private float currentDistance;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private bool isInitialize = false;

    public void Initialize(IInput input, CameraData data)
    {
        target = new GameObject("CameraTarget").transform;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
        this.input = input;
        this.data = data;
        currentDistance = data.MaximumCameraDistance;
        target.position = data.StartCameraPosition;
        offset = (transform.position - target.position).normalized * currentDistance;
        Vector3 angles = transform.eulerAngles;
        currentX = angles.x;
        currentY = angles.y;
        Subscribe();
        isInitialize = true;
    }

    private void LateUpdate()
    {
        if (isInitialize)
        {
            transform.position = target.position + offset;
            UpdateCameraRotation();
        }
    }

    private void UpdateCameraRotation()
    {
        Quaternion rotation = Quaternion.Euler(currentX, currentY, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * currentDistance);

        transform.position = position;
        transform.LookAt(target);
    }

    private void RotateCamera(Vector2 direction)
    {
        direction *= data.CameraRotateSpeed;
        direction.y = -direction.y;
        currentY += direction.x;
        currentX = Mathf.Clamp(currentX + direction.y, data.MinimumXAngle, data.MaximumXAngle);
    }

    private void MoveCamera(Vector3 moveDirection)
    {
        Vector3 newDirection = -moveDirection * data.CameraMovementSpeed;
        target.position += newDirection;
    }

    private void ChangeHeight(float heightAxis)
    {
        if (transform.position.y >= data.MaximumCameraDistance && heightAxis > 0 ||
            transform.position.y <= data.MinimumCameraDistance && heightAxis < 0)
            return;


        currentDistance = Mathf.Clamp(currentDistance - heightAxis * data.CameraZoomSpeed, data.MinimumCameraDistance, data.MaximumCameraDistance);
        offset = offset.normalized * currentDistance;
    }

    private void Subscribe()
    {
        input.Moved += MoveCamera;
        input.Rotated += RotateCamera;
        input.Scroll += ChangeHeight;
    }

    private void Unsubscribe()
    {
        input.Moved -= MoveCamera;
        input.Rotated -= RotateCamera;
        input.Scroll -= ChangeHeight;
    }

    private void OnEnable()
    {
        if (input != null) Subscribe();
    }

    private void OnDisable()
    {
        if (input != null) Unsubscribe();
    }
}
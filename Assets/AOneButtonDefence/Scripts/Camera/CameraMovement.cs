using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    private CameraData data;
    private Transform target;
    private CinemachineVirtualCamera virtualCamera;
    private IInput input;
    private float currentDistance;
    private float currentX;
    private float currentY;
    private Vector3 rotationVelocity;
    private bool isInitialize;

    public void Initialize(IInput input, CameraData data)
    {
        target = new GameObject("CameraTarget").transform;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
        this.input = input;
        this.data = data;
        currentDistance = data.MaximumCameraDistance;
        target.position = data.StartCameraTargetPosition;

        Vector3 angles = transform.eulerAngles;
        currentX = angles.x;
        currentY = angles.y;
        
        UpdateCameraPositionAndRotation();
        Subscribe();
        isInitialize = true;
    }

    private void LateUpdate()
    {
        if (isInitialize)
        {
            UpdateCameraPositionAndRotation();
        }
    }

    private void UpdateCameraPositionAndRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(currentX, currentY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, data.CameraRotationSmooth * Time.deltaTime);

        Vector3 position = target.position - (transform.rotation * Vector3.forward * currentDistance);
        transform.position = position;
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
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * moveDirection.z + right * moveDirection.x) * data.CameraMovementSpeed;
        target.position += movement;
    }

    private void ChangeHeight(float heightAxis)
    {
        currentDistance = Mathf.Clamp(currentDistance - heightAxis * data.CameraZoomSpeed, data.MinimumCameraDistance, data.MaximumCameraDistance);
    }
    
    private void EnableCamera() => virtualCamera.enabled = true;
    
    private void DisableCamera() => virtualCamera.enabled = false;

    private void Subscribe()
    {
        input.Moved += MoveCamera;
        input.Rotated += RotateCamera;
        input.Scroll += ChangeHeight;
        DialogState.AnimatableDialogueStarted += DisableCamera;
        DialogState.AnimatableDialogueEnded += EnableCamera;
        SkinPanel.ShopEnabled += DisableCamera;
        SkinPanel.ShopDisabled += EnableCamera;
    }

    private void Unsubscribe()
    {
        input.Moved -= MoveCamera;
        input.Rotated -= RotateCamera;
        input.Scroll -= ChangeHeight;
        DialogState.AnimatableDialogueStarted -= DisableCamera;
        DialogState.AnimatableDialogueEnded -= EnableCamera;
        SkinPanel.ShopEnabled -= DisableCamera;
        SkinPanel.ShopDisabled -= EnableCamera;
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

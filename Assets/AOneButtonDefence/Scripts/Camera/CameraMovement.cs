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
    private bool canMove = true;

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
        if (isInitialize && canMove)
        {
            UpdateCameraPositionAndRotation();
        }
    }
    
    private void UpdateCameraPositionAndRotation()
    { 
        Vector3 targetPosition = target.position - (Quaternion.Euler(currentX, currentY, 0) * Vector3.forward * currentDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, data.CameraRotationSmooth * Time.deltaTime);
        Quaternion targetRotation = Quaternion.Euler(currentX, currentY, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, data.CameraRotationSmooth * Time.deltaTime);
    }


    private void RotateCamera(Vector2 direction)
    {
        if (canMove == false)
            return;
        
        direction *= data.CameraRotateSpeed;
        direction.y = -direction.y;
        currentY += direction.x;
        currentX = Mathf.Clamp(currentX + direction.y, data.MinimumXAngle, data.MaximumXAngle);
    }

    private void MoveCamera(Vector3 moveDirection)
    {
        if (canMove == false)
            return;
        
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
        if (canMove == false)
            return;
        
        currentDistance = Mathf.Clamp(currentDistance - heightAxis * data.CameraZoomSpeed, data.MinimumCameraDistance, data.MaximumCameraDistance);
    }
    
    private void EnableCamera() => canMove = true;
    
    private void DisableCamera() => canMove = false;

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

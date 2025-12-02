using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    private CameraData data;
    private Transform targetObject;
    private Transform target;
    private Transform followTarget;

    private CinemachineVirtualCamera virtualCamera;
    private IInput input;

    private float maxRotationSpeed;
    private float currentDistance;
    private float currentX;
    private float currentY;

    private bool isInitialized;
    private bool canMove = true;
    private bool targetChanged = false;

    public void Initialize(IInput input, CameraData data)
    {
        targetObject = new GameObject("CameraTarget").transform;
        target = targetObject;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = target;
        virtualCamera.LookAt = targetObject;

        this.input = input;
        this.data = data;

        currentDistance = data.MaximumCameraDistance;
        target.position = data.StartCameraTargetPosition;
        maxRotationSpeed = data.CameraRotateSpeed;

        Vector3 angles = transform.eulerAngles;
        currentX = angles.x;
        currentY = angles.y;

        UpdateCameraPosition();
        Subscribe();
        isInitialized = true;
    }

    private void LateUpdate()
    {
        if (!isInitialized) return;

        if (followTarget != null)
        {
            targetObject.position = Vector3.Lerp(
                targetObject.position,
                followTarget.position,
                data.CameraRotationSmooth * Time.deltaTime);

            ClampTargetPosition();
        }

        UpdateCameraPosition();
        UpdateCameraRotation();
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition =
            target.position - (Quaternion.Euler(currentX, currentY, 0) *
            Vector3.forward * currentDistance);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            data.CameraRotationSmooth * Time.deltaTime);
    }

    private void UpdateCameraRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(currentX, currentY, 0f);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            data.CameraRotationSmooth * Time.deltaTime);

        targetObject.rotation = transform.rotation;
    }

    private void RotateCamera(Vector2 direction)
    {
        if (!canMove && !targetChanged) return;

        direction *= data.CameraRotateSpeed;
        direction.y = -direction.y;

        float maxDelta = data.MaxRotationSpeed * Time.deltaTime;

        float deltaY = Mathf.Clamp(direction.x, -maxDelta, maxDelta);
        float deltaX = Mathf.Clamp(direction.y, -maxDelta, maxDelta);

        currentY += deltaY;
        currentX = Mathf.Clamp(currentX + deltaX, data.MinimumXAngle, data.MaximumXAngle);
    }

    private void MoveCamera(Vector3 moveDirection)
    {
        if (!canMove) return;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * moveDirection.z + right * moveDirection.x) * data.CameraMovementSpeed;

        target.position += movement;

        ClampTargetPosition();
    }

    private void ChangeHeight(float heightAxis)
    {
        if (!canMove && !targetChanged) return;

        currentDistance = Mathf.Clamp(
            currentDistance - heightAxis * data.CameraZoomSpeed,
            data.MinimumCameraDistance,
            data.MaximumCameraDistance
        );
    }

    private void ChangeTarget(Transform newRealTarget)
    {
        DisableCameraTargetMovement();
        followTarget = newRealTarget;
        targetChanged = true;
    }

    private void ResetTarget()
    {
        followTarget = null;
        EnableCameraTargetMovement();
        targetChanged = false;
    }

    private void EnableCameraTargetMovement() => canMove = true;
    private void DisableCameraTargetMovement() => canMove = false;

    private void ClampTargetPosition()
    {
        Vector3 pos = target.position;

        pos.x = Mathf.Clamp(pos.x, data.LimitX.x, data.LimitX.y);
        pos.z = Mathf.Clamp(pos.z, data.LimitZ.x, data.LimitZ.y);

        target.position = pos;
    }

    private void Subscribe()
    {
        input.Moved += MoveCamera;
        input.Rotated += RotateCamera;
        input.Scroll += ChangeHeight;

        DialogState.AnimatableDialogueStarted += DisableCameraTargetMovement;
        DialogState.AnimatableDialogueEnded += EnableCameraTargetMovement;

        SkinPanel.ShopEnabled += DisableCameraTargetMovement;
        SkinPanel.ShopDisabled += EnableCameraTargetMovement;

        PlayerController.CharacterEnabled += ChangeTarget;
        PlayerController.CharacterDisabled += ResetTarget;
    }

    private void Unsubscribe()
    {
        input.Moved -= MoveCamera;
        input.Rotated -= RotateCamera;
        input.Scroll -= ChangeHeight;

        DialogState.AnimatableDialogueStarted -= DisableCameraTargetMovement;
        DialogState.AnimatableDialogueEnded -= EnableCameraTargetMovement;

        SkinPanel.ShopEnabled -= DisableCameraTargetMovement;
        SkinPanel.ShopDisabled -= EnableCameraTargetMovement;

        PlayerController.CharacterEnabled -= ChangeTarget;
        PlayerController.CharacterDisabled -= ResetTarget;
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
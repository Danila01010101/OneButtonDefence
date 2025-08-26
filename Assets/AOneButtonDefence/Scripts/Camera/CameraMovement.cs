using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    private CameraData data;
    private Transform targetObject;   // точка, за которой следует камера
    private Transform target;         // всегда == targetObject (для vcam)
    private Transform followTarget;   // цель, к которой targetObject тянется

    private CinemachineVirtualCamera virtualCamera;
    private IInput input;
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

        Vector3 angles = transform.eulerAngles;
        currentX = angles.x;
        currentY = angles.y;

        // Привести targetObject к начальной позиции/повороту
        targetObject.position = target.position;
        targetObject.rotation = Quaternion.Euler(currentX, currentY, 0f);

        UpdateCameraRotation(); // сразу выставить вращение
        UpdateCameraPosition(); // сразу вычислить позицию

        Subscribe();
        isInitialized = true;
    }

    private void LateUpdate()
    {
        if (!isInitialized) return;

        // 1) Плавно двигаем targetObject к реальному таргету (если есть)
        if (followTarget != null)
        {
            // можно использовать отдельный коэффициент сглаживания, если нужно
            targetObject.position = Vector3.Lerp(
                targetObject.position,
                followTarget.position,
                data.CameraRotationSmooth * Time.deltaTime
            );
        }

        // 2) Сначала обновляем поворот камеры (исходя из currentX/currentY)
        UpdateCameraRotation();

        // 3) Затем обновляем позицию камеры (с использованием уже установленного transform.rotation)
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // вычисляем цель позицию камеры, используя текущий поворот transform.rotation
        Vector3 targetPosition = target.position - (transform.rotation * Vector3.forward * currentDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, data.CameraRotationSmooth * Time.deltaTime);
    }

    private void UpdateCameraRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(currentX, currentY, 0f);

        // Применяем поворот к самой камере (transform) — это важно
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, data.CameraRotationSmooth * Time.deltaTime);

        // И синхронизируем поворот targetObject с камерой — гарантия, что LookAt/Follower видят ту же ориентацию
        targetObject.rotation = transform.rotation;
    }

    private void RotateCamera(Vector2 direction)
    {
        // Ротация разрешена если canMove == true ИЛИ если мы в режиме привязки к target (targetChanged == true)
        // Если ты хочешь запретить вращение при targetChanged — поменяй условие наоборот.
        if (!canMove && !targetChanged) return;

        direction *= data.CameraRotateSpeed;
        direction.y = -direction.y;
        currentY += direction.x;
        currentX = Mathf.Clamp(currentX + direction.y, data.MinimumXAngle, data.MaximumXAngle);
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
        // двигаем именно targetObject (target == targetObject)
        target.position += movement;
    }

    private void ChangeHeight(float heightAxis)
    {
        if (!canMove || targetChanged) return;

        currentDistance = Mathf.Clamp(
            currentDistance - heightAxis * data.CameraZoomSpeed,
            data.MinimumCameraDistance,
            data.MaximumCameraDistance
        );
    }

    private void ChangeTarget(Transform newRealTarget)
    {
        DisableCameraTargetMovement();
        followTarget = newRealTarget; // targetObject плавно едет к новой цели
        targetChanged = true;
    }

    private void ResetTarget()
    {
        followTarget = null; // возвращаем управление в руки игрока
        EnableCameraTargetMovement();
        targetChanged = false;
    }

    private void EnableCameraTargetMovement() => canMove = true;
    private void DisableCameraTargetMovement() => canMove = false;

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
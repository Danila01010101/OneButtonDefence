using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class TrailerCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;
    public float rotationSmooth = 5f;

    private CinemachineVirtualCamera virtualCamera;
    private Transform target;

    private float yaw;
    private float pitch;

    private bool isInitialized = false;
    private bool canMove = true;

    private void Start()
    {
        // Создаем цель для камеры
        target = new GameObject("CameraTarget").transform;
        target.position = transform.position;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;

        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized || !canMove)
            return;

        HandleInput();
    }

    private void LateUpdate()
    {
        if (!isInitialized || !canMove)
            return;

        UpdateCamera();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * lookSpeed;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
        }

        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.E))
            moveDirection.y += 1;
        if (Input.GetKey(KeyCode.Q))
            moveDirection.y -= 1;

        MoveCamera(moveDirection);
    }

    private void UpdateCamera()
    {
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);

        // Камера всегда находится в позиции цели
        transform.position = target.position;
    }

    private void MoveCamera(Vector3 direction)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        Vector3 move = (forward * direction.z + right * direction.x + up * direction.y) * moveSpeed * Time.deltaTime;
        target.position += move;
    }

    private void EnableCamera() => canMove = true;
    private void DisableCamera() => canMove = false;
}

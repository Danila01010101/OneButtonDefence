using AOneButtonDefence.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerControllerData data;
    private Transform cameraTransform;
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    
    private readonly Vector3 gravityDirection = Vector3.down;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        if (cameraTransform == null) Debug.Log("No camera have been assigned for character movement, used main camera.");
        cameraTransform = cameraTransform == null ? Camera.main.transform : cameraTransform;

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }

    public void Initialize(PlayerControllerData data, Transform cameraTransform)
    {
        this.data = data;
        this.cameraTransform = cameraTransform;
    }

    public void Enable()
    {
        if (controller != null) controller.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
        enabled = true;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (controller != null) controller.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        enabled = false;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (!enabled || !playerInput.enabled) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, data.RotationSpeed * Time.deltaTime);
        }

        controller.Move(moveDirection.normalized * (data.MoveSpeed * Time.deltaTime));
        controller.SimpleMove(gravityDirection * (Physics.gravity.y * Time.deltaTime));
    }
}
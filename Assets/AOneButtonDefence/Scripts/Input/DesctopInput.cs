using System;
using UnityEngine;

public class DesctopInput : IInput, IDisableableInput
{
    public Action<Vector2> Clicked { get; set; }
    public Action<Vector3> Moved { get; set; }
    public Action<Vector2> Rotated { get; set; }
    public Action RotateStarted { get; set; }
    public Action RotateEnded { get; set; }
    public Action<float> Scroll { get; set; }
    public static Action MovementEnabled { get; set; }
    public static Action MovementDisabled { get; set; }

    private float deadZone = 0.1f;
    private float clickMaxTime = 0.5f;
    private Vector3 lastMousePosition;
    private float mouseClickButtonTouchTime;
    private bool enabled;
    
    private bool isRotating = false;
    private bool wasCursorLocked = false;

    private readonly string xMoveAxis = "Mouse X";
    private readonly string yMoveAxis = "Mouse Y";
    private readonly string scrollWeelName = "Mouse ScrollWheel";
    private readonly int spellCastButton = 0;
    private readonly int moveMouseButton = 0;
    private readonly int rotateMouseButton = 1;

    public DesctopInput(float deadZone, float clickMaxTime)
    {
        this.deadZone = deadZone;
        this.clickMaxTime = clickMaxTime;
    }

    public void Update()
    {
        if (Input.GetMouseButton(spellCastButton) && Time.time - mouseClickButtonTouchTime < clickMaxTime)
        {
            Clicked?.Invoke(Input.mousePosition);
            Debug.Log("ClickHappened");
        }
        
        HandleMoveInput();
        HandleRotateInput();
    }

    public void LateUpdate()
    {
        if (enabled == false)
            return;
        
        KeyboardMoveInput();
        HandleScrollInput();
    }
    
    public void Enable()
    {
        lastMousePosition = Input.mousePosition;
        enabled = true;
        MovementEnabled?.Invoke();
    }

    public void Disable()
    {
        enabled = false;
        MovementDisabled?.Invoke();
        
        if (isRotating)
        {
            EndRotation();
        }
    }

    private void HandleMoveInput()
    {
        if (Input.GetMouseButtonDown(moveMouseButton))
        {
            lastMousePosition = Input.mousePosition;
            mouseClickButtonTouchTime = Time.time;
        }

        if (Input.GetMouseButton(moveMouseButton))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 moveDirection = new Vector3(-delta.x, 0, -delta.y) * Time.deltaTime;
            Moved?.Invoke(moveDirection);
            lastMousePosition = Input.mousePosition;
        }
    }
    
    private void KeyboardMoveInput()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Moved?.Invoke(moveDirection);
    }
    
    private void HandleRotateInput()
    {
        bool rotateButtonPressed = Input.GetMouseButton(rotateMouseButton);
        
        if (rotateButtonPressed && !isRotating)
        {
            StartRotation();
        }
        else if (!rotateButtonPressed && isRotating)
        {
            EndRotation();
        }
        
        if (isRotating)
        {
            float mouseX = Input.GetAxis(xMoveAxis) * Time.deltaTime;
            float mouseY = Input.GetAxis(yMoveAxis) * Time.deltaTime;
            Rotated?.Invoke(new Vector2(mouseX, mouseY));
        }
    }

    private void StartRotation()
    {
        isRotating = true;
        
        wasCursorLocked = Cursor.lockState == CursorLockMode.Locked;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        RotateStarted?.Invoke();
        
        Debug.Log("Rotation started - Cursor locked");
    }

    private void EndRotation()
    {
        isRotating = false;
        
        if (!wasCursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        RotateEnded?.Invoke();
        
        Debug.Log("Rotation ended - Cursor unlocked");
    }

    private void HandleScrollInput()
    {
        float scrollAxis = Input.GetAxis(scrollWeelName);

        if (scrollAxis != 0)
            Scroll?.Invoke(scrollAxis);
    }
}
using System;
using UnityEngine;

public class DesctopInput : IInput, IDisableableInput
{
    public Action<Vector3> Moved { get; set; }
    public Action<Vector2> Rotated { get; set; }
    public Action<float> Scroll { get; set; }
    public static Action MovementEnabled { get; set; }
    public static Action MovementDisabled { get; set; }

    private float deadZone = 0.1f;
    private Vector3 lastMousePosition;
    private bool enabled;

    private readonly string xMoveAxis = "Mouse X";
    private readonly string yMoveAxis = "Mouse Y";
    private readonly string scrollWeelName = "Mouse ScrollWheel";
    private readonly int moveMouseButton = 0;
    private readonly int rotateMouseButton = 1;

    public DesctopInput(float deadZone)
    {
        this.deadZone = deadZone;
    }

    public void LateUpdate()
    {
        if (enabled == false)
            return;
        
        HandleMoveInput();
        HandleRotateInput();
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
    }

    private void HandleMoveInput()
    {
        if (Input.GetMouseButtonDown(moveMouseButton))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(moveMouseButton))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 moveDirection = new Vector3(-delta.x, 0, -delta.y) * Time.deltaTime;
            Moved?.Invoke(moveDirection);
            lastMousePosition = Input.mousePosition;
        }
    }

    private void HandleRotateInput()
    {
        if (Input.GetMouseButton(rotateMouseButton))
        {
            float mouseX = Input.GetAxis(xMoveAxis) * Time.deltaTime;
            float mouseY = Input.GetAxis(yMoveAxis) * Time.deltaTime;
            Rotated.Invoke(new Vector2(mouseX, mouseY));
        }
    }

    private void HandleScrollInput()
    {
        float scrollAxis = Input.GetAxis(scrollWeelName);

        if (scrollAxis != 0)
            Scroll?.Invoke(scrollAxis);
    }
}
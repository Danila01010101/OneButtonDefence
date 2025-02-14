using System;
using UnityEngine;

public class MobileInput : IInput, IDisableableInput
{
    public Action<Vector2> Clicked { get; set; }
    public Action<Vector3> Moved { get; set; }
    public Action<Vector2> Rotated { get; set; }
    public Action<float> Scroll { get; set; }
    public static Action MovementEnabled { get; set; }
    public static Action MovementDisabled { get; set; }

    private float deadZone = 5f;
    private float clickMaxTime = 0.2f;
    private float lastTapTime;
    private Vector2 lastTouchPosition;
    private bool enabled;

    public MobileInput(float deadZone, float clickMaxTime)
    {
        this.deadZone = deadZone;
        this.clickMaxTime = clickMaxTime;
    }

    public void Update()
    {
        if (!enabled) return;

        HandleTapInput();
        HandleMoveInput();
        HandleRotateInput();
        HandleZoomInput();
    }

    public void LateUpdate() { }

    public void Enable()
    {
        enabled = true;
        MovementEnabled?.Invoke();
    }

    public void Disable()
    {
        enabled = false;
        MovementDisabled?.Invoke();
    }

    private void HandleTapInput()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            float tapDuration = Time.time - lastTapTime;
            if (tapDuration < clickMaxTime)
                Clicked?.Invoke(Input.GetTouch(0).position);
        }
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            lastTapTime = Time.time;
        }
    }

    private void HandleMoveInput()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            if (touchDelta.magnitude > deadZone)
            {
                Vector3 moveDirection = new Vector3(-touchDelta.x, 0, -touchDelta.y) * Time.deltaTime;
                Moved?.Invoke(moveDirection);
            }
        }
    }

    private void HandleRotateInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 delta0 = touch0.deltaPosition;
            Vector2 delta1 = touch1.deltaPosition;

            Vector2 averageDelta = (delta0 + delta1) / 2f;

            Rotated?.Invoke(averageDelta * Time.deltaTime);
        }
    }

    private void HandleZoomInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
            float currentDistance = (touch0.position - touch1.position).magnitude;
            float zoomDelta = currentDistance - prevDistance;

            if (Mathf.Abs(zoomDelta) > deadZone)
            {
                Scroll?.Invoke(zoomDelta * 0.01f);
            }
        }
    }
}

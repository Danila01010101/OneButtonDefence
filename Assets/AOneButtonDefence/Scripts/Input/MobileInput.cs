using System;
using UnityEngine;

public class MobileInput : IInput, IDisableableInput
{
    public Action<Vector2> Clicked { get; set; }
    public Action<Vector3> Moved { get; set; }
    public Action<Vector2> Rotated { get; set; }
    public Action RotateStarted { get; set; }
    public Action RotateEnded { get; set; }
    public Action<float> Scroll { get; set; }
    public static Action MovementEnabled { get; set; }
    public static Action MovementDisabled { get; set; }

    private float deadZone = 5f;
    private float clickMaxTime = 0.2f;
    private float lastTapTime;
    private bool enabled;
    private bool isZooming;
    
    private bool isRotating = false;
    private Vector2 lastRotationTouch0Pos;
    private Vector2 lastRotationTouch1Pos;
    private float rotationStartTime = 0f;
    private float rotationActivationDelay = 0.1f;

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

        if (Input.touchCount == 2)
        {
            bool wasZooming = isZooming;
            isZooming = IsPinchGesture();
            
            if (isZooming)
            {
                if (isRotating)
                {
                    EndRotation();
                }
                HandleZoomInput();
            }
            else
            {
                if (wasZooming && !isZooming)
                {
                    ResetRotationState();
                }
                
                CheckAndHandleRotation();
            }
        }
        else
        {
            if (isRotating)
            {
                EndRotation();
            }
            isZooming = false;
            ResetRotationState();
        }
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
        
        if (isRotating)
        {
            EndRotation();
        }
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

    private void CheckAndHandleRotation()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            lastRotationTouch0Pos = touch0.position;
            lastRotationTouch1Pos = touch1.position;
            rotationStartTime = Time.time;
            return;
        }

        bool canStartRotation = Time.time - rotationStartTime >= rotationActivationDelay;
        
        Vector2 delta0 = touch0.position - lastRotationTouch0Pos;
        Vector2 delta1 = touch1.position - lastRotationTouch1Pos;
        
        bool isRotationGesture = IsRotationGesture(delta0, delta1);

        if (isRotationGesture && canStartRotation && !isRotating)
        {
            StartRotation();
        }

        if (isRotating)
        {
            HandleRotationInput(touch0, touch1);
        }

        lastRotationTouch0Pos = touch0.position;
        lastRotationTouch1Pos = touch1.position;
    }

    private void HandleRotationInput(Touch touch0, Touch touch1)
    {
        Vector2 delta0 = touch0.deltaPosition;
        Vector2 delta1 = touch1.deltaPosition;

        Vector2 averageDelta = (delta0 + delta1) / 2f;
        
        Rotated?.Invoke(averageDelta * Time.deltaTime);
    }

    private void StartRotation()
    {
        isRotating = true;
        RotateStarted?.Invoke();
        Debug.Log("Mobile Rotation started");
    }

    private void EndRotation()
    {
        isRotating = false;
        RotateEnded?.Invoke();
        Debug.Log("Mobile Rotation ended");
    }

    private void ResetRotationState()
    {
        isRotating = false;
        lastRotationTouch0Pos = Vector2.zero;
        lastRotationTouch1Pos = Vector2.zero;
        rotationStartTime = 0f;
    }

    private bool IsRotationGesture(Vector2 delta0, Vector2 delta1)
    {
        if (delta0.magnitude < deadZone || delta1.magnitude < deadZone)
            return false;
            
        float dot = Vector2.Dot(delta0.normalized, delta1.normalized);
        
        return dot > 0.7f;
    }

    private void HandleZoomInput()
    {
        isZooming = true;
        
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

    private bool IsPinchGesture()
    {
        if (Input.touchCount < 2) return false;

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        float prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
        float currentDistance = (touch0.position - touch1.position).magnitude;
        
        Vector2 delta0 = touch0.position - (touch0.position - touch0.deltaPosition);
        Vector2 delta1 = touch1.position - (touch1.position - touch1.deltaPosition);
        
        bool isPinchDirection = Vector2.Dot(delta0.normalized, delta1.normalized) < -0.3f;
        
        return Mathf.Abs(currentDistance - prevDistance) > deadZone && isPinchDirection;
    }
}
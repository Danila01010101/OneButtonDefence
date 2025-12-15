using System;
using UnityEngine;

public interface IInput
{
    Action<Vector2> Clicked  { get; set; }
    Action<Vector3> Moved { get; set; }
    Action<Vector2> Rotated { get; set; }
    public Action RotateStarted { get; set; }
    public Action RotateEnded { get; set; }
    Action<float> Scroll { get; set; }
    static Action MovementEnabled { get; set; }
    static Action MovementDisabled { get; set; }
    void Update();
    void LateUpdate();
}
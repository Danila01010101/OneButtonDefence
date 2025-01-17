using System;
using UnityEngine;

public interface IInput
{
    Action<Vector3> Moved { get; set; }
    Action<Vector2> Rotated { get; set; }
    Action<float> Scroll { get; set; }
    static Action MovementEnabled { get; set; }
    static Action MovementDisabled { get; set; }
    void LateUpdate();
}
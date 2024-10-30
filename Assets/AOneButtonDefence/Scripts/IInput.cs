using System;
using UnityEngine;

public interface IInput
{
    Action<Vector3> CameraMoved { get; }
    Action<float> Scroll { get; }
}
using System;
using UnityEngine;

public class DesctopInput : MonoBehaviour, IInput
{
    public Action<Vector3> CameraMoved { get; }
    public Action<float> Scroll { get; }

    private void Update()
    {

    }
}
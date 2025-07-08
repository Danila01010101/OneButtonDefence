using System;
using UnityEngine;

public class TutorialObject : MonoBehaviour, ITutorialGO
{
    [SerializeField] private GameObject pointerTarget;
    [SerializeField] private string message;
    [SerializeField] private float duration = 0;
    [SerializeField] private int index;
    [SerializeField] private bool isActivated;

    public static Action TaskTriggerActivated;

    public GameObject PointerTarget => pointerTarget == null ? gameObject : pointerTarget;

    public string Message => message;

    public int Index => index;

    public float Duration => duration;

    public bool IsActivated => isActivated;

    public void TriggerTaskFinished() => isActivated = true;

    public void TriggerStartTutorial() 
    {
        if (isActivated == false) TaskTriggerActivated?.Invoke(); 
    }
}

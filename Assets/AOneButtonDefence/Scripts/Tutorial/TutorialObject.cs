using System;
using UnityEngine;

public class TutorialObject : MonoBehaviour, ITutorialGO
{
    [SerializeField] private GameObject pointerTarget;
    [SerializeField] private string message;
    [SerializeField] private float duration = 0;
    [SerializeField] private int index;

    public bool IsActivated { get; private set; }
    public static Action TaskTriggerActivated;

    public GameObject PointerTarget => pointerTarget == null ? gameObject : pointerTarget;

    public string Message => message;

    public int Index => index;

    public float Duration => duration;
    
    public void TriggerTaskFinished() => IsActivated = true;

    public void TriggerStartTutorial() 
    {
        if (IsActivated == false) TaskTriggerActivated?.Invoke();
    }
}

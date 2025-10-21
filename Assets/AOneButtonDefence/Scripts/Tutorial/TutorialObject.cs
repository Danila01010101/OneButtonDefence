using System;
using UnityEngine;

public class TutorialObject : MonoBehaviour, ITutorialGO, IDisposable
{
    [SerializeField] private TutorialSequenceConfig sequenceConfig;
    [SerializeField] private GameObject pointerTarget;
    [SerializeField] private string message;
    [SerializeField] private float duration = 0;
    [SerializeField] private int index;
    [SerializeField] private bool IsActivatedFromStart = true;
    
    protected bool isActivated = false;

    public bool IsActivated => IsActivatedFromStart || isActivated;

    public GameObject PointerTarget => pointerTarget == null ? gameObject : pointerTarget;

    public string Message => message;

    public int Index => index;

    public float Duration => duration;
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, $"Tutorial #{index}");
    }
#endif

    public virtual void Dispose() { }
}

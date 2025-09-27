using UnityEngine;

public class WorldPointer : MonoBehaviour
{
    private Transform target;
    private RectTransform pointer;
    private Camera pointerCamera;

    public void Initialize(Transform target, RectTransform pointer)
    {
        this.target = target;
        this.pointer = pointer;
        pointerCamera = Camera.main;
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 screenPos = pointerCamera.WorldToScreenPoint(target.position);
        Vector2 dir = (screenPos - pointer.position).normalized;
        pointer.right = dir;
        pointer.anchoredPosition = -dir * 50f;
    }
}
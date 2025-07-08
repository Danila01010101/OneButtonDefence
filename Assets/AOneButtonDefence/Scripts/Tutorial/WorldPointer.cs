using UnityEngine;

public class WorldPointer : MonoBehaviour
{
    private Transform target;
    private RectTransform pointer;
    private Camera camera;

    public void Initialize(Transform target, RectTransform pointer)
    {
        this.target = target;
        this.pointer = pointer;
        camera = Camera.main;
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 screenPos = camera.WorldToScreenPoint(target.position);
        Vector2 dir = (screenPos - pointer.position).normalized;
        pointer.right = dir;
        pointer.anchoredPosition = -dir * 50f;
    }
}
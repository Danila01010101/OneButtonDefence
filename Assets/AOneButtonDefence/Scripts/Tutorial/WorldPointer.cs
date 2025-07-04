using UnityEngine;

public class WorldPointer : MonoBehaviour
{
    private Transform _target;
    private RectTransform _pointer;
    private Camera _camera;

    public void Initialize(Transform target, RectTransform pointer)
    {
        _target = target;
        _pointer = pointer;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_target == null) return;

        Vector3 screenPos = _camera.WorldToScreenPoint(_target.position);
        Vector2 dir = (screenPos - _pointer.position).normalized;
        _pointer.right = dir;
        _pointer.anchoredPosition = -dir * 50f;
    }
}
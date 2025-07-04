using UnityEngine;

public class UIPointer : MonoBehaviour
{
    private RectTransform _target;
    private RectTransform _pointer;

    public void Initialize(RectTransform target, RectTransform pointer)
    {
        _target = target;
        _pointer = pointer;
    }

    private void Update()
    {
        if (_target == null) return;

        Vector2 dir = (_target.position - _pointer.position).normalized;
        _pointer.right = dir; // Поворачиваем указатель
        _pointer.anchoredPosition = -dir * 50f; // Смещаем от края
    }
}
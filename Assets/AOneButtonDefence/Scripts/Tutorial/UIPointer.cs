using UnityEngine;

public class UIPointer : MonoBehaviour
{
    private RectTransform target;
    private RectTransform pointer;

    public void Initialize(RectTransform target, RectTransform pointer)
    {
        this.target = target;
        this.pointer = pointer;
    }

    private void Update()
    {
        if (target == null) return;

        Vector2 dir = (target.position - pointer.position).normalized;
        pointer.right = dir; // Поворачиваем указатель
        pointer.anchoredPosition = -dir * 50f; // Смещаем от края
    }
}
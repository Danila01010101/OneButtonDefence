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
        pointer.anchoredPosition = dir * 150f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pointer.rotation = Quaternion.Euler(0, 0, angle - 135f);
    }
}
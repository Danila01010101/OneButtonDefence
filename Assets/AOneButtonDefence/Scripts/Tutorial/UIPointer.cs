using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIPointer : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float approachDuration = 0.8f;
    [SerializeField] private float retreatDuration = 0.4f;
    [SerializeField] private float approachOffset = 30f; 
    [SerializeField] private float edgeMargin = 10f;
    
    private RectTransform target;
    private RectTransform pointer;
    private RectTransform tutorialRect;
    private Canvas canvas;
    private RectTransform canvasRect;
    private Sequence pointerSequence;
    private Vector2 startAnchoredPosition;
    private bool isInitialized = false;

    public void Initialize(RectTransform target, RectTransform pointer)
    {
        this.target = target;
        this.pointer = pointer;
        this.tutorialRect = pointer.parent as RectTransform;
        this.canvas = pointer.GetComponentInParent<Canvas>();
        this.canvasRect = canvas.GetComponent<RectTransform>();

        var tutorialMessage = pointer.GetComponentInParent<TutorialMessage>();
        if (tutorialMessage != null)
        {
            // можно использовать tutorialMessage.EngePadding при необходимости
        }

        startAnchoredPosition = pointer.anchoredPosition;
        isInitialized = true;

        Canvas.ForceUpdateCanvases();
        if (tutorialRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(tutorialRect);

        CalculateStartPosition();
        CreatePointerAnimation();
    }
    
    private void CalculateStartPosition()
    {
        var canvas = pointer.GetComponentInParent<Canvas>();
        var cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

        Vector2 tutorialScreenPoint = RectTransformUtility.WorldToScreenPoint(cam, tutorialRect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tutorialRect, tutorialScreenPoint, cam, out Vector2 tutorialLocalPos);

        Vector2 closestOnTutorial = GetClosestPointOnRect(tutorialRect, target.position, cam);

        Vector2 closestOnTarget = GetClosestPointOnRect(target, tutorialRect.position, cam);

        float biasToTarget = 0.35f;
        Vector2 midPoint = Vector2.Lerp(closestOnTarget, closestOnTutorial, biasToTarget);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            pointer.parent as RectTransform,
            midPoint,
            cam,
            out startAnchoredPosition);

        pointer.anchoredPosition = startAnchoredPosition;
    }
    
    private Vector2 GetClosestPointOnRect(RectTransform rect, Vector3 worldPos, Camera cam)
    {
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);

        Vector2 worldPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPos);

        float minX = Mathf.Min(worldCorners[0].x, worldCorners[2].x);
        float maxX = Mathf.Max(worldCorners[0].x, worldCorners[2].x);
        float minY = Mathf.Min(worldCorners[0].y, worldCorners[2].y);
        float maxY = Mathf.Max(worldCorners[0].y, worldCorners[2].y);

        float clampedX = Mathf.Clamp(worldPoint.x, minX, maxX);
        float clampedY = Mathf.Clamp(worldPoint.y, minY, maxY);

        return new Vector2(clampedX, clampedY);
    }

    private void CreatePointerAnimation()
    {
        if (!isInitialized || target == null || pointer == null || tutorialRect == null || canvasRect == null) return;

        pointerSequence?.Kill();

        var cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

        Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(cam, target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, targetScreenPos, cam, out Vector2 targetCanvasLocal);

        Vector2 tutorialScreenPos = RectTransformUtility.WorldToScreenPoint(cam, tutorialRect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, tutorialScreenPos, cam, out Vector2 tutorialCanvasLocal);

        float halfCanvasW = canvasRect.rect.width * 0.5f;
        float halfCanvasH = canvasRect.rect.height * 0.5f;

        float padding =  (pointer.GetComponentInParent<TutorialMessage>()?.EngePadding) ?? 40f;
        Vector2 clampedTarget = new Vector2(
            Mathf.Clamp(targetCanvasLocal.x, -halfCanvasW + padding + edgeMargin, halfCanvasW - padding - edgeMargin),
            Mathf.Clamp(targetCanvasLocal.y, -halfCanvasH + padding + edgeMargin, halfCanvasH - padding - edgeMargin)
        );

        Vector2 v = clampedTarget - tutorialCanvasLocal;
        float totalDistance = v.magnitude;
        if (totalDistance < 0.1f)
        {
            pointer.anchoredPosition = Vector2.zero;
            UpdatePointerRotationImmediate(tutorialCanvasLocal, targetCanvasLocal);
            return;
        }

        Vector2 dir = v.normalized;

        Vector2 startCanvas = tutorialCanvasLocal + dir * (totalDistance * 0.85f);

        Vector2 endCanvas = clampedTarget; // второй вариант — endCanvas -= dir * someOffset;

        float approachDelta = Mathf.Min(approachOffset, totalDistance * 0.6f);
        Vector2 approachCanvas = startCanvas - dir * approachDelta;

        Vector2 approachLocal = approachCanvas - tutorialCanvasLocal;
        Vector2 endLocal = endCanvas - tutorialCanvasLocal;

        pointer.anchoredPosition = approachLocal;
        UpdatePointerRotationImmediate(tutorialCanvasLocal, targetCanvasLocal);

        pointerSequence = DOTween.Sequence()
            .Append(pointer.DOAnchorPos(endLocal, approachDuration).SetEase(Ease.OutSine))
            .AppendInterval(0.08f)
            .Append(pointer.DOAnchorPos(approachLocal, retreatDuration).SetEase(Ease.OutSine))
            .SetLoops(-1)
            .OnUpdate(() =>
            {
                UpdatePointerRotation();
            });
    }

    private void UpdatePointerRotationImmediate(Vector2 tutorialCanvasLocal, Vector2 targetCanvasLocal)
    {
        Vector2 pointerCanvasPos = tutorialCanvasLocal + pointer.anchoredPosition;
        Vector2 dir = (targetCanvasLocal - pointerCanvasPos).normalized;
        if (dir.sqrMagnitude < 0.0001f) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        pointer.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdatePointerRotation()
    {
        if (!isInitialized || target == null || pointer == null || canvasRect == null || tutorialRect == null) return;

        var cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

        Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(cam, target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, targetScreenPos, cam, out Vector2 targetCanvasLocal);
        Vector2 tutorialScreenPos = RectTransformUtility.WorldToScreenPoint(cam, tutorialRect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, tutorialScreenPos, cam, out Vector2 tutorialCanvasLocal);

        Vector2 pointerCanvasPos = tutorialCanvasLocal + pointer.anchoredPosition;
        Vector2 dir = (targetCanvasLocal - pointerCanvasPos).normalized;
        if (dir.sqrMagnitude < 0.0001f) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        pointer.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnDestroy()
    {
        pointerSequence?.Kill();
    }

    public void StopAnimation()
    {
        pointerSequence?.Kill();
        if (isInitialized && pointer != null)
        {
            pointer.anchoredPosition = startAnchoredPosition;
        }
    }
}
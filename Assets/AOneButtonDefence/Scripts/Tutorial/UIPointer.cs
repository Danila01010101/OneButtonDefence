using DG.Tweening;
using UnityEngine;

public class UIPointer : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float approachDuration = 1f;
    [SerializeField] private float retreatDuration = 0.3f;
    [SerializeField] private float retreatLenght = 125;

    private RectTransform target;
    private RectTransform pointer;
    private Sequence pointerSequence;
    private Vector2 startAnchoredPosition;
    private bool isInitialized = false;
    private float fullDistance;
    private float oneThirdDistance;

    public void Initialize(RectTransform target, RectTransform pointer)
    {
        this.target = target;
        this.pointer = pointer;
        startAnchoredPosition = pointer.anchoredPosition;
        isInitialized = true;

        CalculateDistance();
        CreatePointerAnimation();
    }

    private void CalculateDistance()
    {
        Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(
            pointer.GetComponentInParent<Canvas>().worldCamera,
            target.position
        );

        Vector2 pointerScreenPos = RectTransformUtility.WorldToScreenPoint(
            pointer.GetComponentInParent<Canvas>().worldCamera,
            pointer.position
        );
        
        fullDistance = Vector2.Distance(targetScreenPos, pointerScreenPos);
        oneThirdDistance = fullDistance / 3f;
    }

    private void CreatePointerAnimation()
    {
        if (!isInitialized || target == null || pointer == null) return;

        pointerSequence?.Kill();

        Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(
            pointer.GetComponentInParent<Canvas>().worldCamera,
            target.position
        );
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            pointer.parent as RectTransform,
            targetScreenPos,
            pointer.GetComponentInParent<Canvas>().worldCamera,
            out Vector2 targetAnchoredPos
        );
        
        Vector2 direction = (targetAnchoredPos - startAnchoredPosition).normalized;

        Vector2 retreatPos = direction * (oneThirdDistance * 2.75f);

        Vector2 approachPos = retreatPos - direction * retreatLenght;

        pointer.anchoredPosition = direction * (oneThirdDistance * 2.75f);

        pointerSequence = DOTween.Sequence()
            .Append(pointer.DOAnchorPos(approachPos, approachDuration).SetEase(Ease.OutSine))
            .AppendInterval(0.1f)
            .Append(pointer.DOAnchorPos(retreatPos, retreatDuration).SetEase(Ease.OutSine))
            //.OnUpdate(UpdatePointerRotation)
            .SetLoops(-1);
    }

    private void UpdatePointerRotation()
    {
        if (!isInitialized || target == null || pointer == null) return;

        Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(
            pointer.GetComponentInParent<Canvas>().worldCamera,
            target.position
        );

        Vector2 pointerScreenPos = RectTransformUtility.WorldToScreenPoint(
            pointer.GetComponentInParent<Canvas>().worldCamera,
            pointer.position
        );

        Vector2 direction = (targetScreenPos - pointerScreenPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 135f;
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
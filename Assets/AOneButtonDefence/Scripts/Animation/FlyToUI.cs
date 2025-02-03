using System;
using UnityEngine;
using DG.Tweening;

public class FlyToUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float endScale;

    /// <summary>
    /// Запускает анимацию полета объекта из мира к UI-элементу.
    /// </summary>
    public void Fly(RectTransform uiTarget, Camera uiCamera, float endScale = 1, float duration = 1, Action onComplete = null)
    {
        UIGameObjectShower.Instance.TeleportToUICamera(gameObject, Camera.main);
        Vector2 targetScreenPosition = uiTarget.position;

        Vector3 targetWorldPosition = uiCamera.ScreenToWorldPoint(
            new Vector3(targetScreenPosition.x, targetScreenPosition.y, uiCamera.nearClipPlane + 100f)
        );
        
        Debug.Log(targetScreenPosition);
        Sequence flySequence = DOTween.Sequence();
        flySequence.Append(transform.DOMove(targetWorldPosition, duration).SetEase(Ease.OutQuad));
        flySequence.Join(transform.DOScale(Vector3.one * endScale, duration).SetEase(Ease.OutQuad));

        flySequence.OnComplete(() =>
        {
            onComplete?.Invoke();
            Destroy(gameObject);
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 3);
    }
}
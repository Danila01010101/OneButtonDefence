using System;
using UnityEngine;
using DG.Tweening;

public class FlyToUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private float endScale;
    [SerializeField] private float duration;

    private void Start()
    {
        UIGameObjectShower.Instance.TeleportToUICamera(gameObject, Camera.main);
        Fly(rectTransform, uiCamera);
    }

    /// <summary>
    /// Запускает анимацию полета объекта из мира к UI-элементу.
    /// </summary>
    /// <param name="startWorldPosition">Начальная позиция в мире.</param>
    /// <param name="uiTarget">Целевой UI-элемент (RectTransform).</param>
    /// <param name="mainCamera">Камера, через которую происходит преобразование координат.</param>
    /// <param name="endScale">Конечный масштаб объекта.</param>
    /// <param name="duration">Длительность анимации.</param>
    /// <param name="onComplete">Действие, выполняемое по завершении.</param>
    public void Fly(RectTransform uiTarget, Camera uiCamera, Action onComplete = null)
    {
        // Получаем экранную позицию целевого UI элемента
        Vector2 targetScreenPosition = uiTarget.position;

        // Конвертируем экранную позицию в мировые координаты, но относительно UI камеры
        Vector3 targetWorldPosition = uiCamera.ScreenToWorldPoint(
            new Vector3(targetScreenPosition.x, targetScreenPosition.y, uiCamera.nearClipPlane + 100f)
        );
        Debug.Log(targetScreenPosition);

        // Создаем последовательность анимации
        Sequence flySequence = DOTween.Sequence();

        // Анимация перемещения к целевой позиции
        flySequence.Append(transform.DOMove(targetWorldPosition, duration).SetEase(Ease.OutQuad));

        // Анимация изменения масштаба
        flySequence.Join(transform.DOScale(Vector3.one * endScale, duration).SetEase(Ease.OutQuad));

        // Завершаем анимацию
        flySequence.OnComplete(() =>
        {
            onComplete?.Invoke(); // Вызываем callback, если он задан
            Destroy(gameObject); // Удаляем объект после завершения
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 3);
    }
}
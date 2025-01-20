using UnityEngine;

public class FlyToUI : MonoBehaviour
{
    private Vector3 targetScreenPos;
    private Vector3 worldTargetPos;
    private Transform objectTransform;
    private RectTransform uiTarget;
    private Camera mainCamera;
    private Canvas canvas;
    private System.Action onComplete;

    public float moveSpeed = 5f;
    public float scaleSpeed = 2f;
    public float endScale = 0.1f;

    private bool isMoving = false;

    public void Initialize(Vector3 startWorldPosition, RectTransform uiTarget, Camera mainCamera, Canvas canvas, System.Action onComplete)
    {
        this.uiTarget = uiTarget;
        this.mainCamera = mainCamera;
        this.canvas = canvas;
        this.onComplete = onComplete;

        transform.position = startWorldPosition;
        targetScreenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, uiTarget.position);

        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Перевод координат UI элемента в мировые
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                targetScreenPos,
                mainCamera,
                out worldTargetPos
            );

            // Двигаем объект к целевой позиции
            transform.position = Vector3.Lerp(transform.position, worldTargetPos, moveSpeed * Time.deltaTime);

            // Масштабируем объект
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * endScale, scaleSpeed * Time.deltaTime);

            // Проверяем, достиг ли объект целевой точки
            if (Vector3.Distance(transform.position, worldTargetPos) < 0.01f)
            {
                isMoving = false;
                onComplete?.Invoke(); // Уведомляем о завершении
            }
        }
    }
}
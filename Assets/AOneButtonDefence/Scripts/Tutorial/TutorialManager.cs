using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialMessage tutorialPrefab;
    [SerializeField] private Canvas canvas;

    private static TutorialManager instance;

    public static Action TutorialTriggered;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Initialize(Canvas canvas) => this.canvas = canvas;

    public void ShowTutorial(ITutorialGO tutorialObject, Action onComplete = null)
    {
        var tutorial = Instantiate(tutorialPrefab, canvas.transform);
        PositionTutorial(tutorial.GetComponent<RectTransform>(), tutorialObject.PointerTarget, tutorial.Spacing, tutorial.EngePadding);
        tutorial.Setup(tutorialObject.PointerTarget, tutorialObject.Message, onComplete);
        SpotlightTutorialMask.SetNewTarget(tutorialObject.PointerTarget.GetComponent<RectTransform>());

        if (tutorialObject.Duration > 0)
            Destroy(tutorial.gameObject, tutorialObject.Duration);
    }
    
    private void PositionTutorial(RectTransform tutorialRect, GameObject target, float spacing, float edgePadding)
    {
        var cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
        var canvasRect = canvas.GetComponent<RectTransform>();

        // Сохраняем оригинальные anchor'ы и временно ставим центр
        Vector2 originalAnchorMin = tutorialRect.anchorMin;
        Vector2 originalAnchorMax = tutorialRect.anchorMax;
        tutorialRect.anchorMin = tutorialRect.anchorMax = new Vector2(0.5f, 0.5f);

        Vector2 screenPos;
        if (target.TryGetComponent<RectTransform>(out var targetRect))
        {
            screenPos = targetRect.position;
        }
        else
        {
            screenPos = RectTransformUtility.WorldToScreenPoint(cam, target.transform.position);
        }

        // Центр экрана (в пикселях)
        float centerX = Screen.width * 0.5f;
        float centerY = Screen.height * 0.5f;

        // Переводим в координаты с центром в (0,0)
        Vector2 centeredScreenPos = new Vector2(screenPos.x - centerX, screenPos.y - centerY);

        float halfWidth = tutorialRect.rect.width * 0.5f;
        float halfHeight = tutorialRect.rect.height * 0.5f;

        float minX = -centerX + edgePadding + halfWidth;
        float maxX = centerX - edgePadding - halfWidth;
        float minY = -centerY + edgePadding + halfHeight;
        float maxY = centerY - edgePadding - halfHeight;

        // Четыре потенциальных позиции относительно центра экрана
        Vector2[] candidates = new Vector2[4];
        candidates[0] = new Vector2(centeredScreenPos.x + spacing + halfWidth, centeredScreenPos.y); // Right
        candidates[1] = new Vector2(centeredScreenPos.x - spacing - halfWidth, centeredScreenPos.y); // Left
        candidates[2] = new Vector2(centeredScreenPos.x, centeredScreenPos.y + spacing + halfHeight); // Top
        candidates[3] = new Vector2(centeredScreenPos.x, centeredScreenPos.y - spacing - halfHeight); // Bottom

        Vector2 chosenPos = candidates[0]; // default

        foreach (var candidate in candidates)
        {
            if (candidate.x >= minX && candidate.x <= maxX &&
                candidate.y >= minY && candidate.y <= maxY)
            {
                chosenPos = candidate;
                break;
            }
        }

        chosenPos.x = Mathf.Clamp(chosenPos.x, minX, maxX);
        chosenPos.y = Mathf.Clamp(chosenPos.y, minY, maxY);

        tutorialRect.anchoredPosition = chosenPos;

        // Возвращаем anchor обратно
        tutorialRect.anchorMin = originalAnchorMin;
        tutorialRect.anchorMax = originalAnchorMax;
    }
    
    public static void TriggerTutorial()
    {
        TutorialTriggered?.Invoke();
    }
    
    public static List<ITutorialGO> GetTutorialObjects()
    {
        return FindObjectsByType<TutorialObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .Cast<ITutorialGO>()
            .ToList();
    }
}
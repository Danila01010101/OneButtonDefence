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
        tutorial.Setup(tutorialObject.PointerTarget, tutorialObject.Message);

        if (tutorialObject.Duration > 0)
            Destroy(tutorial.gameObject, tutorialObject.Duration);
    
        if (onComplete != null)
            tutorial.OnClosed += onComplete;
    }
    
    private void PositionTutorial(RectTransform tutorialRect, GameObject target, float spacing, float edgePadding)
    {
        var cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
        var targetRect = target.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        targetRect.GetWorldCorners(corners);
        Vector3 centerWorld = (corners[0] + corners[2]) * 0.5f;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, centerWorld);
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().rect.size;

        float sideOffset = spacing + (tutorialRect.rect.width * 0.5f);
        float leftX = screenPos.x - sideOffset;
        float rightX = screenPos.x + sideOffset;

        bool preferRight = screenPos.x < canvasSize.x / 2f;

        float finalX = preferRight ? Mathf.Min(rightX, canvasSize.x - tutorialRect.rect.width * 0.5f - edgePadding)
            : Mathf.Max(leftX, tutorialRect.rect.width * 0.5f + edgePadding);

        Vector2 finalScreenPos = new Vector2(finalX, screenPos.y);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), finalScreenPos, cam, out var localPos);

        tutorialRect.anchoredPosition = localPos;
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
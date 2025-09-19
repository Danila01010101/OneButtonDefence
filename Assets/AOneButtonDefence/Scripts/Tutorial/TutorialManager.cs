using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialMessage tutorialPrefab;
    [SerializeField] private Canvas canvas;

    private static TutorialManager instance;

    public static Action TutorialStepStarted;

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
        TriggerTutorial();

        var tutorial = Instantiate(tutorialPrefab, canvas.transform);

        tutorial.Setup(tutorialObject.PointerTarget, tutorialObject.Message, onComplete);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tutorial.GetComponent<RectTransform>());

        PositionTutorial(tutorial.GetComponent<RectTransform>(), tutorialObject.PointerTarget, tutorial.Spacing, tutorial.EngePadding);

        tutorial.InitializePointer();

        SpotlightTutorialMask.SetNewTarget(tutorialObject.PointerTarget.GetComponent<RectTransform>());

        if (tutorialObject.Duration > 0)
            StartCoroutine(DestroyAfterRealtime(tutorial.gameObject, tutorialObject.Duration));
    }

    public static List<ITutorialGO> GetTutorialObjects()
    {
        return FindObjectsByType<TutorialObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .Cast<ITutorialGO>()
            .ToList();
    }

    public void EndTutorial()
    {
        var activeTutorials = FindObjectsByType<TutorialObject>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var tutorial in activeTutorials)
        {
            if (tutorial != null)
            {
                tutorial.Dispose();
                Destroy(tutorial);
            }
        }
    }
    
    private void PositionTutorial(RectTransform tutorialRect, GameObject target, float spacing, float edgePadding)
    {
        if (tutorialRect == null || target == null || canvas == null)
            return;

        var cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
        var canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 canvasCenter = Vector2.zero;

        RectTransform targetRect = target.GetComponent<RectTransform>();
        Vector2 screenPos;
        if (targetRect != null)
            screenPos = RectTransformUtility.WorldToScreenPoint(cam, targetRect.position);
        else
            screenPos = RectTransformUtility.WorldToScreenPoint(cam, target.transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out Vector2 targetLocal);

        float halfCanvasWidth = canvasRect.rect.width * 0.5f;
        float halfCanvasHeight = canvasRect.rect.height * 0.5f;

        float halfWidth = tutorialRect.rect.width * 0.5f;
        float halfHeight = tutorialRect.rect.height * 0.5f;

        float minX = -halfCanvasWidth + edgePadding + halfWidth;
        float maxX = halfCanvasWidth - edgePadding - halfWidth;
        float minY = -halfCanvasHeight + edgePadding + halfHeight;
        float maxY = halfCanvasHeight - edgePadding - halfHeight;

        Vector2 dirToCenter = (canvasCenter - targetLocal).normalized;

        Vector2 candidate = targetLocal + dirToCenter * spacing;

        Vector2 offset = new Vector2(dirToCenter.x * halfWidth, dirToCenter.y * halfHeight);
        candidate += offset;

        candidate.x = Mathf.Clamp(candidate.x, minX, maxX);
        candidate.y = Mathf.Clamp(candidate.y, minY, maxY);

        tutorialRect.anchorMin = tutorialRect.anchorMax = new Vector2(0.5f, 0.5f);
        tutorialRect.anchoredPosition = candidate;
    }

    private static void TriggerTutorial()
    {
        TutorialStepStarted?.Invoke();
    }

    private IEnumerator DestroyAfterRealtime(GameObject obj, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (obj != null)
            Destroy(obj);
    }
}
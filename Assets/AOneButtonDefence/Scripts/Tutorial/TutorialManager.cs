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

        Vector2 originalAnchorMin = tutorialRect.anchorMin;
        Vector2 originalAnchorMax = tutorialRect.anchorMax;
        tutorialRect.anchorMin = tutorialRect.anchorMax = new Vector2(0.5f, 0.5f);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tutorialRect);

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, target.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out Vector2 targetLocal);

        float halfCanvasWidth = canvasRect.rect.width * 0.5f;
        float halfCanvasHeight = canvasRect.rect.height * 0.5f;

        float halfWidth = tutorialRect.rect.width * 0.5f;
        float halfHeight = tutorialRect.rect.height * 0.5f;

        float minX = -halfCanvasWidth + edgePadding + halfWidth;
        float maxX = halfCanvasWidth - edgePadding - halfWidth;
        float minY = -halfCanvasHeight + edgePadding + halfHeight;
        float maxY = halfCanvasHeight - edgePadding - halfHeight;

        Vector2[] candidates = new Vector2[4];
        candidates[0] = new Vector2(targetLocal.x + spacing + halfWidth, targetLocal.y);
        candidates[1] = new Vector2(targetLocal.x - spacing - halfWidth, targetLocal.y);
        candidates[2] = new Vector2(targetLocal.x, targetLocal.y + spacing + halfHeight);
        candidates[3] = new Vector2(targetLocal.x, targetLocal.y - spacing - halfHeight);

        Vector2 chosenPos = candidates[0];

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

        tutorialRect.anchorMin = originalAnchorMin;
        tutorialRect.anchorMax = originalAnchorMax;
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
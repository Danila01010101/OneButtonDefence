using UnityEngine;
using System;

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

    public void ShowTutorial(ITutorialGO tutorialObject,
        Action onComplete = null)
    {
        var tutorial = Instantiate(tutorialPrefab, canvas.transform);
        tutorial.Setup(tutorialObject.PointerTarget, tutorialObject.Message);

        if (tutorialObject.Duration > 0) Destroy(tutorial.gameObject, tutorialObject.Duration);
        if (onComplete != null) tutorial.OnClosed += onComplete;
    }

    public static void TriggerTutorial()
    {
        TutorialTriggered?.Invoke();
    }
}
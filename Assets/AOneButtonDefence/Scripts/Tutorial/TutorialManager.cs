using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialMessage tutorialPrefab;
    [SerializeField] private Canvas canvas;

    public void Initialize(Canvas canvas) => this.canvas = canvas;

    public void ShowTutorial(ITutorialGO tutorialObject,
                           Action onComplete = null)
    {
        var tutorial = Instantiate(tutorialPrefab, canvas.transform);
        tutorial.Setup(tutorialObject.PointerTarget, tutorialObject.Message);

        if (tutorialObject.Duration > 0) Destroy(tutorial.gameObject, tutorialObject.Duration);
        if (onComplete != null) tutorial.OnClosed += onComplete;
    }
}
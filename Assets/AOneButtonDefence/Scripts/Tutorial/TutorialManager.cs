using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialMessage _tutorialPrefab;
    [SerializeField] private Canvas _canvas;

    public void ShowTutorial(GameObject target, string message,
                           Action onComplete = null,
                           float duration = 0)
    {
        var tutorial = Instantiate(_tutorialPrefab, _canvas.transform);
        tutorial.Setup(target, message);

        if (duration > 0) Destroy(tutorial.gameObject, duration);
        if (onComplete != null) tutorial.OnClosed += onComplete;
    }
}
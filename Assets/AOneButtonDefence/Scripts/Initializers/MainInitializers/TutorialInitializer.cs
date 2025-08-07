using System.Collections;
using UnityEngine;

public class TutorialInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private TutorialManager tutorialManagerPrefab;

    public IEnumerator Initialize()
    {
        var canvas = CanvasInitializer.GameplayCanvas.GetComponent<Canvas>();
        var manager = Instantiate(tutorialManagerPrefab, canvas.transform);
        manager.Initialize(canvas);

        var tutorial = new BasicMechanicsTutorial(manager);

        yield return null;
    }
}
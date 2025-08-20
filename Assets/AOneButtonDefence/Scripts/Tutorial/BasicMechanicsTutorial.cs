using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BasicMechanicsTutorial
{
    private TutorialManager tutorialManager;
    private List<ITutorialGO> objectsForTutorial;
    private int currentTutorialObjectIndex;
    private bool isActivated;

    public BasicMechanicsTutorial(TutorialManager tutorialManager)
    {
        this.tutorialManager = tutorialManager;
        TutorialStartState.TutorialStarted += StartTutorial;
        TutorialManager.TutorialStepStarted += ActivateNextTutorial;
        SetTutorialObjects(TutorialManager.GetTutorialObjects());
    }

    public void SetTutorialObjects(List<ITutorialGO> foundObjectsForTutorial)
    {
        if (objectsForTutorial == null || objectsForTutorial.Count == 0)
        {
            return;
        }
        
        objectsForTutorial = foundObjectsForTutorial.OrderBy(tutorialObject => tutorialObject.Index).ToList();
    } 

    public void StartTutorial()
    {
        objectsForTutorial = TutorialManager.GetTutorialObjects();
        
        if (objectsForTutorial == null || objectsForTutorial.Count == 0) 
            return;

        ShowNextStep();
    }
    
    private void ActivateNextTutorial() => isActivated = true;

    private void ShowNextStep()
    {
        CoroutineStarter.Instance.StartCoroutine(WaitForNextStep());
    }

    private IEnumerator WaitForNextStep()
    {
        var steps = objectsForTutorial.OrderBy(x => x.Index).ToList();

        if (currentTutorialObjectIndex >= steps.Count)
        {
            Debug.Log("Tutorial finished!");
            yield break;
        }

        // Берём текущий шаг
        var step = steps[currentTutorialObjectIndex];
        currentTutorialObjectIndex++;

        // Показываем его и указываем, что после завершения надо вызвать ShowNextStep
        while (step.IsActivated == false)
        {
            yield return null;
        } 
        
        tutorialManager.ShowTutorial(step, ShowNextStep);

        yield return null;
    }
}
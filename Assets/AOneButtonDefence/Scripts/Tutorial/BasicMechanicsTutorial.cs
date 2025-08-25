using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicMechanicsTutorial : IDisposable
{
    private TutorialManager tutorialManager;
    private List<ITutorialGO> objectsForTutorial;
    private int currentTutorialObjectIndex;
    private bool isActivated;
    private Coroutine runningCoroutine;

    public BasicMechanicsTutorial(TutorialManager tutorialManager)
    {
        this.tutorialManager = tutorialManager;
        TutorialStartState.TutorialStarted += StartTutorial;
        TutorialManager.TutorialStepStarted += ActivateNextTutorial;
        TutorialMessage.OnSkipButtonPressed += SkipTutorial;
        SetTutorialObjects(TutorialManager.GetTutorialObjects());
    }

    public void SetTutorialObjects(List<ITutorialGO> foundObjectsForTutorial)
    {
        if (foundObjectsForTutorial == null || foundObjectsForTutorial.Count == 0)
            return;
        
        objectsForTutorial = foundObjectsForTutorial
            .OrderBy(tutorialObject => tutorialObject.Index)
            .ToList();
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
        runningCoroutine = CoroutineStarter.Instance.StartCoroutine(WaitForNextStep());
    }

    private IEnumerator WaitForNextStep()
    {
        var steps = objectsForTutorial.OrderBy(x => x.Index).ToList();

        if (currentTutorialObjectIndex >= steps.Count)
        {
            Debug.Log("Tutorial finished!");
            yield break;
        }

        var step = steps[currentTutorialObjectIndex];
        currentTutorialObjectIndex++;

        while (step.IsActivated == false)
        {
            yield return null;
        } 
        
        tutorialManager.ShowTutorial(step, ShowNextStep);

        yield return null;
    }

    private void SkipTutorial()
    {
        if (runningCoroutine != null)
        {
            CoroutineStarter.Instance.StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        currentTutorialObjectIndex = objectsForTutorial?.Count ?? 0;

        tutorialManager.EndTutorial();

        Debug.Log("Tutorial skipped!");
    }

    public void Dispose()
    {
        TutorialStartState.TutorialStarted -= StartTutorial;
        TutorialManager.TutorialStepStarted -= ActivateNextTutorial;
        TutorialMessage.OnSkipButtonPressed -= SkipTutorial;
    }
}
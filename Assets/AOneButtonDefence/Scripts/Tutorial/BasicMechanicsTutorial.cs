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
        TutorialManager.TutorialTriggered += ActivateNextTutorial;
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

        ShowNextTutorial();
    }
    
    private void ActivateNextTutorial() => isActivated = true;

    private void ShowNextTutorial() => CoroutineStarter.Instance.StartCoroutine(WaitForNextTutorial());

    private IEnumerator WaitForNextTutorial()
    {   
        if (currentTutorialObjectIndex + 1 > objectsForTutorial.Count)
        {
            Debug.Log("Tutorial ended.");
            yield break;
        }

        while (isActivated == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        isActivated = false;
        tutorialManager.ShowTutorial(objectsForTutorial[currentTutorialObjectIndex++], ShowNextTutorial);
        SetTutorialObjects(TutorialManager.GetTutorialObjects());
        yield return null;
    }
}
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

    public BasicMechanicsTutorial(TutorialManager tutorialManager, List<ITutorialGO> objectsForTutorial)
    {
        this.tutorialManager = tutorialManager;
        TutorialStartState.TutorialStarted += StartTutorial;
        TutorialManager.TutorialTriggered += ActivateNextTutorial;
        SetTutorialObjects(objectsForTutorial);
    }

    public void SetTutorialObjects(List<ITutorialGO> objectsForTutorial)
    {
        this.objectsForTutorial = objectsForTutorial.OrderBy(tutorialObject => tutorialObject.Index).ToList();
    } 

    public void StartTutorial()
    {
        tutorialManager.ShowTutorial(objectsForTutorial[0], ShowNextTutorial);
    }
    
    private void ActivateNextTutorial() => isActivated = true;

    private void ShowNextTutorial() => CoroutineStarter.Instance.StartCoroutine(WaitForNextTutorial());

    private IEnumerator WaitForNextTutorial()
    {
        if (++currentTutorialObjectIndex > objectsForTutorial.Count)
        {
            Debug.Log("Tutorial ended.");
            yield break;
        }

        while (isActivated == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        isActivated = false;
        tutorialManager.ShowTutorial(objectsForTutorial[currentTutorialObjectIndex], ShowNextTutorial);
    }
}
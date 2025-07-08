using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BasicMechanicsTutorial
{
    private TutorialManager tutorialManager;
    private List<ITutorialGO> objectsForTutorial;
    private int currentTutoorialObjectIndex;

    public BasicMechanicsTutorial(TutorialManager tutorialManager, UnityAction tutorialStart)
    {
        this.tutorialManager = tutorialManager;
        tutorialStart += StartTutorial;
    }

    public void SetTutorialObjects(List<ITutorialGO> objectsForTutorial)
    {
        this.objectsForTutorial = objectsForTutorial.OrderBy(tutorialObject => tutorialObject.Index).ToList();
    } 

    public void StartTutorial()
    {
        tutorialManager.ShowTutorial(objectsForTutorial[0], ShowNextTutorial);
    }

    private void ShowNextTutorial() => CoroutineStarter.Instance.StartCoroutine(WaitForNextTutorial());

    private IEnumerator WaitForNextTutorial()
    {
        if (++currentTutoorialObjectIndex > objectsForTutorial.Count)
        {
            Debug.Log("Tutorial ended.");
            yield break;
        }

        while (objectsForTutorial[currentTutoorialObjectIndex].IsActivated == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        tutorialManager.ShowTutorial(objectsForTutorial[currentTutoorialObjectIndex], ShowNextTutorial);
    }
}
using UnityEngine;

public class TutorialForResources : MonoBehaviour
{
    private void Awake()
    {
        DialogState.AnimatableDialogueEnded += delegate
        {
            TutorialManager.TriggerTutorial();
            Destroy(this);
        };
    }
}
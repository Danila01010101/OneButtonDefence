using UnityEngine;

public class TutorialForResources : MonoBehaviour
{
    private System.Action handler;
    
    private void Awake()
    {
        handler = delegate
        {
            TutorialManager.TriggerTutorial();
            DialogState.AnimatableDialogueEnded -= handler;
            Destroy(this);
        };

        DialogState.AnimatableDialogueEnded += handler;
    }
}
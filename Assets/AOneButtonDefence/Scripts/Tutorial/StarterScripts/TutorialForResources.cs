using UnityEngine;

public class TutorialForResources : MonoBehaviour
{
    private System.Action handler;
    
    private void Awake()
    {
        handler = delegate
        {
            DialogState.AnimatableDialogueEnded -= handler;
            Destroy(this);
        };

        DialogState.AnimatableDialogueEnded += handler;
    }
}
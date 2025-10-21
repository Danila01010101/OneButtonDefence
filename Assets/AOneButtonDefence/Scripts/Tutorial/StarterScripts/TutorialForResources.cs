using System;
using UnityEngine;

public class TutorialForResources : MonoBehaviour
{
    private System.Action handler;
    private bool unsubscribed = false;
    
    private void Awake()
    {
        handler = delegate
        {
            DialogState.AnimatableDialogueEnded -= handler;
            Destroy(this);
            unsubscribed = true;
        };

        DialogState.AnimatableDialogueEnded += handler;
    }

    private void OnDestroy()
    {
        if (unsubscribed == false)
            DialogState.AnimatableDialogueEnded -= handler;
    }
}
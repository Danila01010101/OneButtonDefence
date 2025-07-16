using UnityEngine;

public interface ITutorialGO
{
    GameObject PointerTarget { get; }
    string Message { get; }
    float Duration { get; }
    int Index { get; }
    void TriggerStartTutorial();
    void TriggerTaskFinished();
}

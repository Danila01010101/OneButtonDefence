using UnityEngine;
using System.Diagnostics;

public class DebugActiveState : MonoBehaviour
{
    private void OnEnable()
    {
        LogStateChange(true);
    }

    private void OnDisable()
    {
        LogStateChange(false);
    }

    private void LogStateChange(bool enabled)
    {
        // StackTrace показывает, откуда был вызов
        StackTrace stackTrace = new StackTrace(2, true);

        string state = enabled ? "ENABLED" : "DISABLED";

        UnityEngine.Debug.Log(
            $"[{state}] GameObject: '{gameObject.name}'\n" +
            $"Called from:\n{stackTrace}",
            this
        );
    }
}
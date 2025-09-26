using UnityEngine;

public class BrowserConsoleLogger : MonoBehaviour
{
    void Awake() {
        Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
    }
    
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // В редакторе просто выводим в обычную консоль
        Debug.unityLogger.logHandler.LogFormat(type, null, "{0}\n{1}", logString, stackTrace);

#if UNITY_WEBGL && !UNITY_EDITOR
        // А в WebGL выводим в нативную консоль браузера
        LogToBrowser(type, logString, stackTrace);
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void BrowserConsoleLog(string str);

    void LogToBrowser(LogType type, string message, string stack)
    {
        // Склеиваем
        string combined = $"[{type}] {message}\n{stack}";
        BrowserConsoleLog(combined);
    }
#endif
}
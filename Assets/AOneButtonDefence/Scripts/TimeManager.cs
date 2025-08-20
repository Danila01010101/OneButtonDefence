using UnityEngine;
using System;

public static class TimeManager
{
    private static float defaultTimeScale = 1f;
    private static float currentTimeScale = 1f;
    private static bool isPaused = false;

    public static event Action<bool> OnPauseStateChanged;
    public static event Action<float> OnTimeScaleChanged;

    static TimeManager()
    {
        ApplyTimeScale(defaultTimeScale);
    }

    private static void ApplyTimeScale(float scale)
    {
        currentTimeScale = scale;
        Time.timeScale = currentTimeScale;
        OnTimeScaleChanged?.Invoke(currentTimeScale);
    }

    public static void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        ApplyTimeScale(0f);
        OnPauseStateChanged?.Invoke(true);
    }

    public static void Resume()
    {
        if (!isPaused) return;
        isPaused = false;
        ApplyTimeScale(defaultTimeScale);
        OnPauseStateChanged?.Invoke(false);
    }

    public static void SetTimeScale(float scale)
    {
        defaultTimeScale = Mathf.Max(0, scale);

        if (!isPaused)
            ApplyTimeScale(defaultTimeScale);
    }

    public static float GetTimeScale()
    {
        return currentTimeScale;
    }

    public static bool IsPaused()
    {
        return isPaused;
    }
}
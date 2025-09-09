using System;
using UnityEngine;

public class SkinChangeDetector : IDisposable
{
    private static SkinChangeDetector instance;
    
    public SkinData CurrentSkinData { get; private set; }
    public bool IsSkinChanged { get; private set; }
    public static SkinChangeDetector Instance;

    public SkinChangeDetector()
    {
        if (Instance != null)
            return;
            
        Instance = this;
        SkinPanel.SkinChanged += DetectSkinChanged;
    }
    
    public void Unsubscribe() => SkinPanel.SkinChanged -= DetectSkinChanged;

    private void DetectSkinChanged(SkinData skin)
    {
        CurrentSkinData = skin;
        IsSkinChanged = true;
    }

    public void Dispose() => Unsubscribe();
}
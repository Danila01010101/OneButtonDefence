using System;

public class SkinChangeDetector : IDisposable
{
    private static SkinChangeDetector _instance;
    public static SkinChangeDetector Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SkinChangeDetector();
            return _instance;
        }
    }

    public SkinData CurrentSkinData { get; private set; }
    public bool IsSkinChanged { get; private set; }

    private SkinChangeDetector()
    {
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
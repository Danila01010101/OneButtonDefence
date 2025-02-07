using UnityEngine;

public class SkinChangeDetector
{
    private static SkinChangeDetector instance;
    
    public Mesh CurrentSkinMesh { get; private set; }
    public Material CurrentSkinMaterial { get; private set; }
    public bool IsSkinChanged { get; private set; }
    public static SkinChangeDetector Instance;

    public SkinChangeDetector()
    {
        if (Instance != null)
            return;
            
        Instance = this;
        SkinPanel.SkinChanged += DetectSkinChanged;
    }

    private void DetectSkinChanged(Mesh mesh, Material material)
    {
        CurrentSkinMesh = mesh;
        CurrentSkinMaterial = material;
        IsSkinChanged = true;
    }
}
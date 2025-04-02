using Unity.VisualScripting;
using UnityEngine;

public class GnomeSkinChanger
{
    private readonly MeshFilter meshFilter;
    private readonly Renderer renderer;

    public GnomeSkinChanger(MeshFilter meshFilter, Renderer renderer)
    {
        this.meshFilter = meshFilter;
        this.renderer = renderer;
        SkinPanel.SkinChanged += ChangeSkin;
		
        if (SkinChangeDetector.Instance.IsSkinChanged)
            ChangeSkin(SkinChangeDetector.Instance.CurrentSkinMesh, SkinChangeDetector.Instance.CurrentSkinMaterial);
    }

    public void Unsubscribe() => SkinPanel.SkinChanged -= ChangeSkin;

    public void ChangeSkin(Mesh newMesh, Material newMaterial)
    {
        if (meshFilter != null)
            meshFilter.mesh = newMesh;
        
        if (renderer != null)
            renderer.material = newMaterial;
    }
}
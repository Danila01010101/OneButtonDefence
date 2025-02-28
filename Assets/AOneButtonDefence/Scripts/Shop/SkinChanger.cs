using Unity.VisualScripting;
using UnityEngine;

public class SkinChanger
{
    private readonly MeshFilter meshFilter;
    private readonly Renderer renderer;

    public SkinChanger(MeshFilter meshFilter, Renderer renderer)
    {
        this.meshFilter = meshFilter;
        this.renderer = renderer;
        SkinPanel.SkinChanged += ChangeSkin;
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
using UnityEngine;

public class GnomeSkinChanger
{
    private readonly MeshFilter meshFilter;
    private readonly Renderer renderer;
    private readonly AudioSource audioSource;

    public GnomeSkinChanger(MeshFilter meshFilter, Renderer renderer, AudioSource audioSource)
    {
        this.meshFilter = meshFilter;
        this.renderer = renderer;
        this.audioSource = audioSource;
        SkinPanel.SkinChanged += ChangeSkin;
		
        if (SkinChangeDetector.Instance.IsSkinChanged)
            ChangeSkin(SkinChangeDetector.Instance.CurrentSkinMesh, SkinChangeDetector.Instance.CurrentSkinMaterial);
    }

    public void Unsubscribe() => SkinPanel.SkinChanged -= ChangeSkin;

    public void ChangeSkin(Mesh newMesh, Material newMaterial, AudioClip newAudioClip)
    {
        if (meshFilter != null)
            meshFilter.mesh = newMesh;
        
        if (renderer != null)
            renderer.material = newMaterial;
    }
}
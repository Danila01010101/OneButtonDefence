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
            ChangeSkin(SkinChangeDetector.Instance.CurrentSkinData);
    }

    public void Unsubscribe() => SkinPanel.SkinChanged -= ChangeSkin;

    public void ChangeSkin(SkinData data)
    {
        if (meshFilter != null)
            meshFilter.mesh = data.Mesh;
        
        if (renderer != null)
            renderer.material = data.Material;
        
        if (audioSource != null)
            audioSource.clip = data.DeathSound;
    }
}
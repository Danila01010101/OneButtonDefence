using System.Collections.Generic;
using UnityEngine;

public class GnomeSkinChanger
{
    public Transform ModelTransform => modelTransform;

    private readonly List<MeshFilter> meshFilters;
    private readonly Renderer renderer;
    private readonly AudioSource audioSource;
    private readonly Transform modelTransform;
    private readonly Transform pivotTarget;

    private Material lockedMaterial;

    public GnomeSkinChanger(List<MeshFilter> meshFilters, Renderer renderer, Transform pivotTarget, AudioSource audioSource = null, Material customLockedMaterial = null)
    {
        this.meshFilters = meshFilters ?? new List<MeshFilter>();
        this.renderer = renderer;
        this.audioSource = audioSource;
        this.modelTransform = (this.meshFilters.Count > 0) ? this.meshFilters[0].transform : null;
        this.pivotTarget = pivotTarget;

        if (customLockedMaterial != null)
        {
            lockedMaterial = customLockedMaterial;
        }
        else
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");
            lockedMaterial = shader != null ? new Material(shader) : new Material(Shader.Find("Sprites/Default"));
            lockedMaterial.SetColor("_BaseColor", Color.black);
            lockedMaterial.color = Color.black;
            lockedMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }
    }

    public void Unsubscribe() { }

    public void ChangeSkin(SkinData data)
    {
        bool unlocked = data != null && data.Unlocked;
        ChangeSkin(data, unlocked);
    }

    public void ChangeSkin(SkinData data, bool unlocked)
    {
        if (data == null) return;

        if (audioSource != null)
            audioSource.clip = data.DeathSound;

        if (meshFilters != null)
        {
            foreach (var mf in meshFilters)
            {
                if (mf == null) continue;
                mf.sharedMesh = data.Mesh;
            }
        }

        if (renderer != null)
        {
            Material matToApply = (unlocked && data.Material != null) ? data.Material : lockedMaterial;
            if (matToApply != null)
                renderer.sharedMaterial = matToApply; // <-- ключевой момент
        }
    }

    public void SetLockedMaterial(Material mat)
    {
        if (mat == null) return;
        lockedMaterial = mat;
    }
}
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

    private readonly Material lockedMaterial;

    public GnomeSkinChanger(List<MeshFilter> meshFilters, Renderer renderer, Transform pivotTarget, AudioSource audioSource = null)
    {
        this.meshFilters = meshFilters;
        this.renderer = renderer;
        this.audioSource = audioSource;
        modelTransform = meshFilters[0].transform;
        this.pivotTarget = pivotTarget;

        lockedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        lockedMaterial.color = Color.black;
        lockedMaterial.SetColor("_BaseColor", Color.black);
        lockedMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Background;
        
        SkinPanel.SkinChanged += ChangeSkin;

        if (SkinChangeDetector.Instance.IsSkinChanged)
            ChangeSkin(SkinChangeDetector.Instance.CurrentSkinData);
    }

    public void Unsubscribe() => SkinPanel.SkinChanged -= ChangeSkin;

    public void ChangeSkin(SkinData data)
    {
        if (audioSource != null)
            audioSource.clip = data.DeathSound;
        
        if (meshFilters != null)
        {
            foreach (var meshRenderer in meshFilters)
            {
                meshRenderer.mesh = data.Mesh;
            }

            if (pivotTarget != null && meshFilters.Count > 0)
            {
                var bounds = meshFilters[0].sharedMesh.bounds;
                Vector3 localBottom = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
                
                foreach (var meshRenderer in meshFilters)
                {
                    Vector3 worldBottom = meshRenderer.transform.TransformPoint(localBottom);
                    Vector3 offset = pivotTarget.position - worldBottom;
                    meshRenderer.transform.position += offset;
                }
            }
        }

        if (renderer != null)
        {
            if (data.Unlocked)
                renderer.material = data.Material;
            else
                renderer.material = lockedMaterial;
        }
    }
}
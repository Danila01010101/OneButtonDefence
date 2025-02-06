using UnityEngine;

public class ExampleSkinChanger
{
    private readonly MeshFilter meshFilter;
    private readonly Renderer renderer;

    public ExampleSkinChanger(MeshFilter meshFilter, Renderer renderer)
    {
        this.meshFilter = meshFilter;
        this.renderer = renderer;
    }

    public void ChangeSkin(Mesh newMesh, Material newMaterial)
    {
        meshFilter.mesh = newMesh;
        renderer.material = newMaterial;
    }
}
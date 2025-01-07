using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public void SetOpaque(Material material)
    {
        material.SetFloat("_Surface", 0); // Установка непрозрачного режима
        material.SetFloat("_Blend", 0);
        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
        material.SetFloat("_ZWrite", 1.0f);
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
    }

    public void SetTransparent(Material material)
    {
        material.SetFloat("_Surface", 1); // Установка прозрачного режима
        material.SetFloat("_Blend", 1);
        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0.0f);
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}

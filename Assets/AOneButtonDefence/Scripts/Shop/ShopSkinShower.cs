using System;
using UnityEngine;

public class ShopSkinShower : MonoBehaviour
{
    [field : SerializeField] public SkinRotator SkinRotator { get; private set; }
    
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Renderer skinRenderer;
    
    public SkinChanger SkinChanger { get; private set; }

    public void Initialize(IInput input)
    {
        SkinChanger = new SkinChanger(meshFilter, skinRenderer);
        SkinRotator.Initialize(input);
    }

    public void ShowExampleSkin() => gameObject.SetActive(true);
    
    public void HideExampleSkin() => gameObject.SetActive(false);

    private void OnDestroy()
    {
        SkinChanger.Unsubscribe();
        SkinChanger = null;
    }
}
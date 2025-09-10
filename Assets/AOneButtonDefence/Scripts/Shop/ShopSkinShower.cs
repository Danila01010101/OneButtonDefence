using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopSkinShower : MonoBehaviour
{
    [field : SerializeField] public SkinRotator SkinRotator { get; private set; }
    
    [SerializeField] private List<MeshFilter> meshFilter;
    [SerializeField] private Renderer skinRenderer;
    [SerializeField] private Transform skinOffsetTransform; 
    
    public GnomeSkinChanger GnomeSkinChanger { get; private set; }

    public void Initialize(IInput input)
    {
        GnomeSkinChanger = new GnomeSkinChanger(meshFilter, skinRenderer, skinOffsetTransform);
        SkinRotator.Initialize(input);
    }

    public void ShowExampleSkin() => gameObject.SetActive(true);
    
    public void HideExampleSkin() => gameObject.SetActive(false);

    private void OnDestroy()
    {
        GnomeSkinChanger.Unsubscribe();
        GnomeSkinChanger = null;
    }
}
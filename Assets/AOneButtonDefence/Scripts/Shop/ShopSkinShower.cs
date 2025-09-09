using System;
using UnityEngine;

public class ShopSkinShower : MonoBehaviour
{
    [field : SerializeField] public SkinRotator SkinRotator { get; private set; }
    
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Renderer skinRenderer;
    [SerializeField] private AudioSource audioSource;
    
    public GnomeSkinChanger GnomeSkinChanger { get; private set; }

    public void Initialize(IInput input)
    {
        GnomeSkinChanger = new GnomeSkinChanger(meshFilter, skinRenderer, audioSource);
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
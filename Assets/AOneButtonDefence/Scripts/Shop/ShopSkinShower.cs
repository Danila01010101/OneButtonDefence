using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShopSkinShower : MonoBehaviour
{
    [field : SerializeField] public SkinRotator SkinRotator { get; private set; }
    
    [SerializeField] private List<MeshFilter> meshFilter;
    [SerializeField] private Renderer skinRenderer;
    [SerializeField] private ParticleSystem SkinParticles; 
    [SerializeField] private Transform skinOffsetTransform; 
    [SerializeField] private GameObject parentGameObject; 
    
    private SkinBoughtEffectsPlayer skinBoughtEffectsPlayer;
    
    public GnomeSkinChanger GnomeSkinChanger { get; private set; }

    public void Initialize(IInput input)
    {
        GnomeSkinChanger = new GnomeSkinChanger(meshFilter, skinRenderer, skinOffsetTransform);
        SkinRotator.Initialize(input);
        skinBoughtEffectsPlayer = new SkinBoughtEffectsPlayer(SkinParticles, GnomeSkinChanger.ModelTransform);
    }

    public void ShowExampleSkin() => parentGameObject.SetActive(true);
    
    public void HideExampleSkin() => parentGameObject.SetActive(false);

    private void OnDestroy()
    {
        GnomeSkinChanger.Unsubscribe();
        GnomeSkinChanger = null;
    }
}
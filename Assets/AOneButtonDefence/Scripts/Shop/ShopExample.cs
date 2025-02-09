using System;
using UnityEngine;

public class ShopExample : MonoBehaviour
{
    [SerializeField] private GameObject gnome;

    public static Action<SkinData> GnomeSkinChanged;

    private void Awake()
    {
	    var skin = new SkinData(gnome, 10);
    }

    public void BuySkin(SkinData skin)
    {
	    if (skin.Cost > ResourcesCounter.Instance.Data.GemsAmount)
		    return;
	    
	    ResourcesCounter.Instance.Data.GemsAmount -= skin.Cost;
	    GnomeSkinChanged?.Invoke(skin);
    }

    public class SkinData
    {
	    private GameObject skin;
	    private Material skinMaterial;
	    public int Cost { get; private set; }

	    public SkinData(GameObject skin, int cost)
	    {
		    this.skin = skin;
		    Cost = cost;
	    }
    }
}
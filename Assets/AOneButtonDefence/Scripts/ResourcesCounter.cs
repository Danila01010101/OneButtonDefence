using System;
using System.Diagnostics;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
	private static ResourcesCounter instance;

    public static event Action<ResourcesData> ResourcesAmountChanged
    {
        add => instance.Data.ResourcesAmountChanged += value;
        remove => instance.Data.ResourcesAmountChanged -= value;
    }

    public static int Materials => instance.Data.Materials;
    public static int FoodAmount => instance.Data.FoodAmount;
    public static int SurvivorSpirit => instance.Data.SurvivorSpirit;
    public static int Warriors => instance.Data.Warriors;
    public static int GemsAmount => instance.Data.GemsAmount;

    public ResourcesData Data { get; private set; }

    private int gnomeDeathFine;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(instance);
        }

        Data = new ResourcesData();
        instance = this;
    }

    private void DetectGnomeDeath() => Data.SurvivorSpirit -= gnomeDeathFine;

    private void DetectSkinBuy(int cost) => Data.GemsAmount -= cost;

    private void UpdateValues() => Data.ResourcesAmountChanged?.Invoke(Data);

    private void OnEnable() 
    {
        SkinPanel.SkinBought += DetectSkinBuy;
        GnomeFightingUnit.GnomeDied += DetectGnomeDeath;
        GameInitializer.GameInitialized += UpdateValues;
    }

    private void OnDisable()
    {
        SkinPanel.SkinBought -= DetectSkinBuy;
        GnomeFightingUnit.GnomeDied -= DetectGnomeDeath;
        GameInitializer.GameInitialized -= UpdateValues;
    }

    public void SetStartValues(int startFood, int startMaterials, int survivorSpirit)
    {
        Data.FoodAmount = startFood;
        Data.Materials = startMaterials;
        Data.SurvivorSpirit = survivorSpirit;
    }
    
    public void SetGnomeDeathFine(int fine) => gnomeDeathFine = fine;

    public class ResourcesData
    {
        public int FoodAmount 
        {
            get => foodAmount;

            set
            {
                foodAmount = value;
                ResourcesAmountChanged?.Invoke(this);
            }
        }
        public int Warriors
        {
            get => warriors;

            set
            {
                warriors = value;
                ResourcesAmountChanged?.Invoke(this);
            }
        }
        
        public int Materials
        {
            get => materials;

            set
            {
                materials = value;
                ResourcesAmountChanged?.Invoke(this);
            }
        }
        public int SurvivorSpirit
        {
            get => survivorSpirit;

            set
            {
                survivorSpirit = value;
                ResourcesAmountChanged?.Invoke(this);
            }
        }

        public int GemsAmount
        {
            get => gemsAmount;

            set
            {
                gemsAmount = value;
                ResourcesAmountChanged?.Invoke(this);
            }
        }

        private int foodAmount;
        private int warriors;
        private int materials;
        private int survivorSpirit;
        private int gemsAmount;

        public Action<ResourcesData> ResourcesAmountChanged;
    }
}
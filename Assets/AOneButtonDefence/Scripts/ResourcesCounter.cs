using System;
using System.Diagnostics;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
	private static ResourcesCounter instance;

    #region StaticActions

    public static event Action<int> FoodAmountChanged
    {
        add => instance.Data.FoodAmountChanged += value;
        remove => instance.Data.FoodAmountChanged -= value;
    }

    public static event Action<int> SpiritAmountChanged
    {
        add => instance.Data.SpiritAmountChanged += value;
        remove => instance.Data.SpiritAmountChanged -= value;
    }

    public static event Action<int> MaterialsAmountChanged
    {
        add => instance.Data.MaterialsAmountChanged += value;
        remove => instance.Data.MaterialsAmountChanged -= value;
    }

    public static event Action<int> WarriorsAmountChanged
    {
        add => instance.Data.WarriorsAmountChanged += value;
        remove => instance.Data.WarriorsAmountChanged -= value;
    }

    public static event Action<int> GemsAmountChanged
    {
        add => instance.Data.GemsAmountChanged += value;
        remove => instance.Data.GemsAmountChanged -= value;
    }

    #endregion

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

    private void OnEnable() 
    {
        SkinPanel.SkinBought += DetectSkinBuy;
        GnomeFightingUnit.GnomeDied += DetectGnomeDeath;
    }

    private void OnDisable()
    {
        SkinPanel.SkinBought -= DetectSkinBuy;
        GnomeFightingUnit.GnomeDied -= DetectGnomeDeath;
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
                FoodAmountChanged?.Invoke(value);
                foodAmount = value;
            }
        }
        public int Warriors
        {
            get => warriors;

            set
            {
                WarriorsAmountChanged?.Invoke(value);
                warriors = value;
            }
        }
        
        public int Materials
        {
            get => materials;

            set
            {
                MaterialsAmountChanged?.Invoke(value);
                materials = value;
            }
        }
        public int SurvivorSpirit
        {
            get => survivorSpirit;

            set
            {
                SpiritAmountChanged?.Invoke(value);
                survivorSpirit = value;
            }
        }

        public int GemsAmount
        {
            get => gemsAmount;

            set
            {
                GemsAmountChanged?.Invoke(value);
                gemsAmount = value;
            }
        }

        private int foodAmount;
        private int warriors;
        private int materials;
        private int survivorSpirit;
        private int gemsAmount;

        public Action<int> FoodAmountChanged;
        public Action<int> WarriorsAmountChanged;
        public Action<int> MaterialsAmountChanged;
        public Action<int> SpiritAmountChanged;
        public Action<int> GemsAmountChanged;
    }
}
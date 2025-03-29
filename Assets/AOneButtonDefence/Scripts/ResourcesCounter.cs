using System;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
	public static ResourcesCounter Instance;

    public ResourcesData Data { get; private set; }

    private int gnomeDeathFine;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(Instance);
        }

        Data = new ResourcesData();
        Instance = this;
    }

    private void DetectGnomeDeath() => Data.SurvivorSpirit -= gnomeDeathFine;

    private void OnEnable() => GnomeFightingUnit.GnomeDied += DetectGnomeDeath;

    private void OnDisable() => GnomeFightingUnit.GnomeDied -= DetectGnomeDeath;

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
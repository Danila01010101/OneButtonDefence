public class FineCounter
{
    public static FineCounter Instance { get; private set; } 
    
    private ResourcesCounter resourcesCounter;
    private int foodFine;
    private int materialsFine;
    private int spiritFine;

    public FineCounter(ResourcesCounter counter) 
    { 
        resourcesCounter = counter; 
        Instance = this;
        Subscribe();
    }

    public void InstantFoodFine(int value) => resourcesCounter.Data.FoodAmount -= value;

    public void InstantMaterialsFine(int value) => resourcesCounter.Data.Materials -= value;

    public void InstantSpiritFine(int value) => resourcesCounter.Data.SurvivorSpirit -= value;

    public void InstantWarriorFine(int value) => resourcesCounter.Data.Warriors -= value;

    public void AddMaterialsFine(int value) => materialsFine += value;

    public void AddSpiritFine(int value) => spiritFine += value;

    public void AddFoodFine(int value) => foodFine += value;

    private void ActivateFine()
    {
        resourcesCounter.Data.FoodAmount -= foodFine;
        resourcesCounter.Data.SurvivorSpirit -= spiritFine;
        resourcesCounter.Data.Materials -= materialsFine;
    }

    private void Subscribe() => UpgradeState.UpgradeStateEnded += ActivateFine;
}
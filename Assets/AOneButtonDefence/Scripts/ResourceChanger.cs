public class ResourceChanger
{
    public static ResourceChanger Instance { get; private set; }
    public int FoodChange { get; private set; }
    public int MaterialsChange { get; private set; }
    public int SpiritChange { get; private set; }
    public int WarriorChange { get; private set; }

    private ResourcesCounter resourcesCounter;

    public ResourceChanger(ResourcesCounter counter)
    {
        resourcesCounter = counter;
        Instance = this;
        Subscribe();
    }

    public void InstantFoodChange(int value) => resourcesCounter.Data.FoodAmount += value;

    public void InstantMaterialsChange(int value) => resourcesCounter.Data.Materials += value;

    public void InstantSpiritChange(int value) => resourcesCounter.Data.SurvivorSpirit += value;

    public void InstantWarriorChange(int value) => resourcesCounter.Data.Warriors += value;

    public void AddFoodPerTurn(int value) => FoodChange += value;

    public void AddMaterialsPerTurn(int value) => MaterialsChange += value;

    public void AddSpiritPerTurn(int value) => SpiritChange += value;

    public void AddWarriorPerTurn(int value) => WarriorChange += value;

    private void ActivateIncome()
    {
        resourcesCounter.Data.FoodAmount += FoodChange;
        resourcesCounter.Data.Materials += MaterialsChange;
        resourcesCounter.Data.SurvivorSpirit += SpiritChange;
        resourcesCounter.Data.Warriors += WarriorChange;
    }

    private void Subscribe() => UpgradeState.UpgradeStateEnding += ActivateIncome;
}

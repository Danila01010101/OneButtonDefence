public class IncomeCounter
{
    public static IncomeCounter Instance { get; private set; }

    private ResourcesCounter resourcesCounter;
    private int foodIncome;
    private int materialsIncome;
    private int spiritIncome;
    private int warriorIncome;

    public IncomeCounter(ResourcesCounter counter)
    {
        resourcesCounter = counter;
        Instance = this;
        Subscribe();
    }

    public void InstantFoodIncome(int value) => resourcesCounter.Data.FoodAmount += value;

    public void InstantMaterialsIncome(int value) => resourcesCounter.Data.Materials += value;

    public void InstantSpiritIncome(int value) => resourcesCounter.Data.SurvivorSpirit += value;

    public void InstantWarriorIncome(int value) => resourcesCounter.Data.Warriors += value;

    public void AddFoodIncome(int value) => foodIncome += value;

    public void AddMaterialsIncome(int value) => materialsIncome += value;

    public void AddSpiritIncome(int value) => spiritIncome += value;

    public void AddWarriorIncome(int value) => warriorIncome += value;

    private void ActivateIncome()
    {
        resourcesCounter.Data.FoodAmount += foodIncome;
        resourcesCounter.Data.Materials += materialsIncome;
        resourcesCounter.Data.SurvivorSpirit += spiritIncome;
        resourcesCounter.Data.Warriors += warriorIncome;
    }

    private void Subscribe() => UpgradeState.UpgradeStateEnded += ActivateIncome;
}

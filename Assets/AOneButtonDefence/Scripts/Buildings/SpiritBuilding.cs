public class SpiritBuilding : Building
{
    private SpiritBuildingData data;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += data.SpawnBonus;
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.SpiritBuildingData;
        cost = data.Cost;
        foodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += data.EveryTurnBonus;
    }
}
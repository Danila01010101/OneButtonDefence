public class Farm : Building
{
    private FarmData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.FoodAmount += data.SpawnBonus;
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.FarmData;
        Cost = data.Cost;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.FoodAmount += data.EveryTurnBonus;
    }
}
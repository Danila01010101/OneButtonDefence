public class Factory : Building
{
    private FactoryData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.Materials += data.SpawnBonus;
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.FactoryData;
        Cost = data.Cost;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.Materials += data.EveryTurnBonus;
    }
}
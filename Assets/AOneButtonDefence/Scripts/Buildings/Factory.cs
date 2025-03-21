public class Factory : Building
{
    private FactoryData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourceChanger.Instance.InstantMaterialsChange(data.SpawnBonus);
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.FactoryData;
        Cost = data.Cost;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void RegisterEndMoveAction()
    {
        base.RegisterEndMoveAction();
        ResourceChanger.Instance.AddMaterialsPerTurn(data.EveryTurnBonus);
    }
}
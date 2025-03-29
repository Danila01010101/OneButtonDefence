public class Factory : Building
{
    private BasicBuildingData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourceChanger.Instance.InstantMaterialsChange(data.SpawnBonus);
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.FactoryData;
        Cost = data.;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void RegisterEndMoveAction()
    {
        base.RegisterEndMoveAction();
        ResourceChanger.Instance.AddMaterialsPerTurn(data.EveryTurnBonus);
    }
}
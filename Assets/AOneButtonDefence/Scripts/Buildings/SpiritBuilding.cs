using System.Collections;

public class SpiritBuilding : Building
{
    private SpiritBuildingData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        IncomeCounter.Instance.InstantSpiritIncome(data.SpawnBonus);
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.SpiritBuildingData;
        Cost = data.Cost;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void RegisterEndMoveAction()
    {
        base.RegisterEndMoveAction();
        IncomeCounter.Instance.AddSpiritIncome(data.EveryTurnBonus);
    }
}
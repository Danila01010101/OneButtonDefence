using System.Collections;

public class SpiritBuilding : Building
{
    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourceIncomeCounter.Instance.InstantSpiritChange(data.SpawnBonus);
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
        ResourceIncomeCounter.Instance.AddSpiritPerTurn(data.EveryTurnBonus);
    }
}
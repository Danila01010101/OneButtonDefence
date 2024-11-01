using System.Collections;

public class SpiritBuilding : Building
{
    private SpiritBuildingData data;

    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += data.SpawnBonus;
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.SpiritBuildingData;
        Cost = data.Cost;
        FoodPerTurnAmount = data.FoodPerTurnAmount;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += data.EveryTurnBonus;
    }
}
public class SpiritBuilding : Building
{
    private SpiritBuildingData data;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += data.SpawnBonus;
    }

    protected override void SetupData()
    {
        base.SetupData();
        data = BuildingsData.SpiritBuildingData;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += data.EveryTurnBonus;
    }
}
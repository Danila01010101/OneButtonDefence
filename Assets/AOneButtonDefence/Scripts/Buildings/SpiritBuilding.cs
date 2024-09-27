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
        base.SetupData(buildingsData);
        data = buildingsData.SpiritBuildingData;
        humanAmount = data.StartHumanAmount;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += data.EveryTurnBonus;
    }
}
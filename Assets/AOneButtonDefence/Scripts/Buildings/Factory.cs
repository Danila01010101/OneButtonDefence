public class Factory : Building
{
    private FactoryData data;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.Materials += data.SpawnBonus;
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        base.SetupData(buildingsData);
        data = buildingsData.FactoryData;
        humanAmount = data.StartHumanAmount;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.Materials += data.EveryTurnBonus;
    }
}
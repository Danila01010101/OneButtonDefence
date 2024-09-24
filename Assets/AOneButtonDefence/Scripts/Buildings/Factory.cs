public class Factory : Building
{
    private FactoryData data;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.Materials += data.SpawnBonus;
    }

    protected override void SetupData()
    {
        base.SetupData();
        data = BuildingsData.FactoryData;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.Materials += data.EveryTurnBonus;
    }
}
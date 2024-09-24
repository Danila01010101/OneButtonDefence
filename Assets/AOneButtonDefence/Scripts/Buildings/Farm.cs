public class Farm : Building
{
    private FarmData data;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.FoodAmount += data.SpawnBonus;
    }

    protected override void SetupData()
    {
        base.SetupData();
        data = BuildingsData.FarmData;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.FoodAmount += data.EveryTurnBonus;
    }
}
public class Farm : Building
{
    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.FoodAmount += 15;
    }

    public override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.FoodAmount += 5;
    }
}
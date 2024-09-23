public class SpiritBuilding : Building
{
    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += 15;
    }

    public override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        ResourcesCounter.Instance.Data.SurvivorSpirit += 15;
    }
}
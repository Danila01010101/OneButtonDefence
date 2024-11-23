public class CampBlock : Ground
{
    public override void Initialize()
    {
        Type = GroundBlockType.CampBlock;
    }

    public override void ActivateBonus()
    {
        base.ActivateBonus();
        //TODO: Spawn warrior
    }
}
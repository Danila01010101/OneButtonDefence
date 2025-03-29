public class WaterBlock : Ground
{
    public override void ActivateBonus()
    {
        base.ActivateBonus();
        ResourceChanger.Instance.InstantFoodChange(20);
    }
}
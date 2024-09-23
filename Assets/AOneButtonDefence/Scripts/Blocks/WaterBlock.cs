public class WaterBlock : Ground
{
    public override void ActivateBonus()
    {
        base.ActivateBonus();
        ResourcesCounter.Instance.Data.FoodAmount += 20;
    }
}
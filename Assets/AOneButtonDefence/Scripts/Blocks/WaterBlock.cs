public class WaterBlock : Ground
{
    public override void ActivateBonus()
    {
        base.ActivateBonus();
        //ResourceIncomeCounter.Instance.InstantFoodChange(20);
    }
}
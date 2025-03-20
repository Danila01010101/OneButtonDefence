public class WaterBlock : Ground
{
    public override void ActivateBonus()
    {
        base.ActivateBonus();
        IncomeCounter.Instance.InstantFoodIncome(20);
    }
}
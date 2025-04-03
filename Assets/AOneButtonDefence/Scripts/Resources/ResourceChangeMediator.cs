public class ResourceChangeMediator
{
    private ResourceAmount gnomeDeathFine;
    private ResourceData gemsResource;

    public ResourceChangeMediator(ResourceAmount gnomeDeathFine, ResourceData gemsResource)
    {
        this.gnomeDeathFine = gnomeDeathFine;
        this.gemsResource = gemsResource;
    }

    private void DetectGnomeDeath() => ResourceIncomeCounter.Instance.InstantResourceChange(gnomeDeathFine);

    private void DetectSkinBuy(int cost) => ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(gemsResource, -cost));

    public void Subscribe() 
    {
        SkinPanel.SkinBought += DetectSkinBuy;
        GnomeFightingUnit.GnomeDied += DetectGnomeDeath;
    }

    public void Unsubscribe()
    {
        SkinPanel.SkinBought -= DetectSkinBuy;
        GnomeFightingUnit.GnomeDied -= DetectGnomeDeath;
    }
}
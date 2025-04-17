public class ResourceChangeMediator
{
    private StartResourceAmount gnomeDeathSpiritFine;
    private StartResourceAmount warriorDeathResource;
    private ResourceData gemsResource;

    public ResourceChangeMediator(StartResourceAmount gnomeDeathSpiritFine, ResourceData gemsResource, StartResourceAmount warriorDeathResource)
    {
        this.gnomeDeathSpiritFine = gnomeDeathSpiritFine;
        this.gemsResource = gemsResource;
        this.warriorDeathResource = warriorDeathResource;
    }

    private void DetectGnomeDeath()
    {
        ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(warriorDeathResource));
        ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(gnomeDeathSpiritFine));
    }

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
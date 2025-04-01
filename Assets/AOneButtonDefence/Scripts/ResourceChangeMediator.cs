public class ResourceChangeMediator
{
    private ResourceAmount gnomeDeathFine;
    private ResourceData gemResource;

    private void DetectGnomeDeath() => ResourceIncomeCounter.Instance.InstantResourceChange(gnomeDeathFine);

    private void DetectSkinBuy(int cost) => ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(gemResource, -cost));

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
    
    public void SetGnomeDeathFine(ResourceAmount fine) => gnomeDeathFine = fine;
    
    public void SetSkinResource(ResourceData resource) => gemResource = resource;
}
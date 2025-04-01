using System.Collections;
using System.Collections.Generic;

public class MilitaryCamp : Building
{
    private MilitaryCampData data;
    private UnitsFactory unitsFactory;
    
    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        StartCoroutine(SpawnWithDelay());
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.MilitaryCampData;
        Cost = data.Cost;
    }

    public void SetupFactory(IEnemyDetector knightDetector)
    {
        unitsFactory = new UnitsFactory(new List<FightingUnit>() { data.GnomeWarriorPrefab }, knightDetector);
    }

    protected override void RegisterEndMoveAction()
    {
        base.RegisterEndMoveAction();
        ResourceIncomeCounter.Instance.AddWarriorPerTurn(data.EveryTurnWarriorsAmount);
    }
    
    private void EveryTurnWarriorsSpawn() => AddWarriors(data.EveryTurnWarriorsAmount);

    private void AddWarriors(int amount)
    {
        FoodPerTurnAmount += amount;
        ResourceIncomeCounter.Instance.InstantWarriorChange(amount);
        
        for (int i = 0; i < amount; i++)
        {
            var spawnedWarrior = unitsFactory.SpawnUnit<GnomeFightingUnit>(transform.position + data.spawnOffset);
            
            if (SkinChangeDetector.Instance.IsSkinChanged)
                spawnedWarrior.ChangeSkin(SkinChangeDetector.Instance.CurrentSkinMesh, SkinChangeDetector.Instance.CurrentSkinMaterial);
        }
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return null;
        AddWarriors(data.StartWarriorsAmount);
        ResourceIncomeCounter.Instance.InstantWarriorChange(data.StartWarriorsAmount);
    }

    protected override void OnEnable()
    {
        base.OnDisable();
        UpgradeState.UpgradeStateEnding += EveryTurnWarriorsSpawn;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UpgradeState.UpgradeStateEnding += EveryTurnWarriorsSpawn;
    }
}
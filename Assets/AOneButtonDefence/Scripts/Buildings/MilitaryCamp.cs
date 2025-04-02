using System.Collections;
using System.Collections.Generic;

public class MilitaryCamp : Building
{
    
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

    protected override void RegisterEndMoveAction()
    {
        base.RegisterEndMoveAction();
        ResourceIncomeCounter.Instance.AddWarriorPerTurn(data.EveryTurnWarriorsAmount);
    }
    
    private void EveryTurnWarriorsSpawn() => AddWarriors(data.EveryTurnWarriorsAmount);

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
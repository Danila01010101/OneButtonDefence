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
        unitsFactory = new UnitsFactory(new List<FightingUnit>() { data.GnomeWarriorPrefab });
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();

        AddWarriors(data.EveryTurnWarriorsAmount);
    }

    private void AddWarriors(int amount)
    {
        FoodPerTurnAmount += amount;
        ResourcesCounter.Instance.Data.Warriors += amount;
        
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
    }
}
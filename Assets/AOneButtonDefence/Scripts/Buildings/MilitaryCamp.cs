using System.Collections;
using UnityEngine;

public class MilitaryCamp : Building
{
    private MilitaryCampData data;
    
    protected override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        StartCoroutine(SpawnWithDelay());
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.MilitaryCampData;
        cost = data.Cost;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();

        AddWarriors(data.EveryTurnWarriorsAmount);
    }

    private void AddWarriors(int amount)
    {
        foodPerTurnAmount += amount;
        ResourcesCounter.Instance.Data.Warriors += amount;
        Instantiate(data.GnomeWarriorPrefab, transform.position + data.spawnOffset, Quaternion.Euler(data.spawnRotation));
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return null;
        AddWarriors(data.StartWarriorsAmount);
    }
}
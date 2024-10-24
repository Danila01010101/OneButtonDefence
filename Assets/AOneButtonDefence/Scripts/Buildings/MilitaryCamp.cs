using UnityEngine;

public class MilitaryCamp : Building
{
    private MilitaryCampData data;
    
    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        AddWarriors(data.StartWarriorsAmount);
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
}
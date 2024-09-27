using UnityEngine;

public class MilitaryCamp : Building
{
    private MilitaryCampData data;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        AddWarrior(data.SpawnWarriorsAmount);
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        base.SetupData(buildingsData);
        data = buildingsData.MilitaryCampData;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        AddWarrior(data.EveryTurnWarriorsAmount);
    }

    private void AddWarrior(int amount)
    {
        humanAmount += amount;
        ResourcesCounter.Instance.Data.Warriors += amount;
    }
}
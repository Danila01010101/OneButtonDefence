using UnityEngine;

public class MilitaryCamp : Building
{
    private MilitaryCampData data;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        AddWarrior(data.StartWarriorsAmount);
    }

    public override void SetupData(BuildingsData buildingsData)
    {
        data = buildingsData.MilitaryCampData;
        cost = data.Cost;
    }

    protected override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();

        AddWarrior(data.EveryTurnWarriorsAmount);
    }

    private void AddWarrior(int amount)
    {
        foodPerTurnAmount += amount;
        ResourcesCounter.Instance.Data.Warriors += amount;
    }
}
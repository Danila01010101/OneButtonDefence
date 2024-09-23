using UnityEngine;

public class MilitaryCamp : Building
{
    [SerializeField] private int spawnWarriorsAmount = 1;

    public override void ActivateSpawnAction()
    {
        base.ActivateSpawnAction();
        AddWarrior(3);
    }

    public override void ActivateEndMoveAction()
    {
        base.ActivateEndMoveAction();
        AddWarrior(spawnWarriorsAmount);
    }

    private void AddWarrior(int amount)
    {
        humanAmount += amount;
        ResourcesCounter.Instance.Data.Warriors += amount;
    }
}
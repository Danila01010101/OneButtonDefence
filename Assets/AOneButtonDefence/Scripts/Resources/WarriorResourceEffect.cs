using UnityEngine;

public class WarriorResourceEffect : IResourceEffect
{
    private readonly UnitsFactory unitsFactory;
    private readonly Vector3 gnomeSpawnOffset;

    public WarriorResourceEffect(UnitsFactory factory, Vector3 gnomeSpawnOffset)
    {
        unitsFactory = factory;
        this.gnomeSpawnOffset = gnomeSpawnOffset;
    }

    public void ApplyEffect(int amount, Vector3? spawnPosition = null)
    {
        if (!spawnPosition.HasValue)
        {
            spawnPosition = Vector3.zero;
        }

        for (int i = 0; i < amount; i++)
        {
            Debug.Log(spawnPosition+ gnomeSpawnOffset);
            var spawnedWarrior = unitsFactory.SpawnUnit<GnomeFightingUnit>(spawnPosition.Value + gnomeSpawnOffset);
            
            if (SkinChangeDetector.Instance.IsSkinChanged)
                spawnedWarrior.ChangeSkin(SkinChangeDetector.Instance.CurrentSkinMesh, SkinChangeDetector.Instance.CurrentSkinMaterial);
        }
    }
}
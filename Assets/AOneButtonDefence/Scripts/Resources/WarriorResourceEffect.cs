using UnityEngine;

public class WarriorResourceEffect : IResourceEffect
{
    private readonly UnitsFactory unitsFactory;

    public WarriorResourceEffect(UnitsFactory factory)
    {
        unitsFactory = factory;
    }

    public void ApplyEffect(int amount, Vector3? spawnPosition = null)
    {
        if (!spawnPosition.HasValue)
        {
            Debug.LogWarning("Попытка заспавнить воинов без позиции! Используется (0,0,0).");
            spawnPosition = Vector3.zero;
        }

        for (int i = 0; i < amount; i++)
        {
            var spawnedWarrior = unitsFactory.SpawnUnit<GnomeFightingUnit>(spawnPosition.Value);
            
            if (SkinChangeDetector.Instance.IsSkinChanged)
                spawnedWarrior.ChangeSkin(SkinChangeDetector.Instance.CurrentSkinMesh, SkinChangeDetector.Instance.CurrentSkinMaterial);
        }
    }
}
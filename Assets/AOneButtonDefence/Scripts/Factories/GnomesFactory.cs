using UnityEngine;

public class GnomesFactory : UnitsFactory
{
    

if (SkinChangeDetector.Instance.IsSkinChanged)
    spawnedWarrior.ChangeSkin(SkinChangeDetector.Instance.CurrentSkinMesh, SkinChangeDetector.Instance.CurrentSkinMaterial);
}
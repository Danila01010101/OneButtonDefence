using System;
using UnityEngine;

public class GnomeFightingUnit : FightingUnit
{
    [SerializeField] protected MeshFilter meshFilter;
    
    private SkinChanger skinChanger;

    public override void Initialize(IEnemyDetector detector)
    {
        base.Initialize(detector);
        skinChanger = new SkinChanger(meshFilter, render);
    }

    private void OnDestroy()
    {
        skinChanger.Unsubscribe();
        skinChanger = null;
    }

    public void ChangeSkin(Mesh newMesh, Material newMaterial) => skinChanger.ChangeSkin(newMesh, newMaterial);
}
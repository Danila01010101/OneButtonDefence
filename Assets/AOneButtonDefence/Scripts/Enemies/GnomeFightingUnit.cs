using System;
using UnityEngine;

public class GnomeFightingUnit : FightingUnit
{
    [SerializeField] protected MeshFilter meshFilter;
    
    private GnomeSkinChanger gnomeSkinChanger;

    public static Action GnomeDied;

    public override void Initialize(IEnemyDetector detector)
    {
        base.Initialize(detector);
        gnomeSkinChanger = new GnomeSkinChanger(meshFilter, render);
    }

    protected override void Die()
    {
        GnomeDied?.Invoke();
        base.Die();
    }

    private void OnDestroy()
    {
        gnomeSkinChanger.Unsubscribe();
        gnomeSkinChanger = null;
    }

    public void ChangeSkin(Mesh newMesh, Material newMaterial) => gnomeSkinChanger.ChangeSkin(newMesh, newMaterial);
}
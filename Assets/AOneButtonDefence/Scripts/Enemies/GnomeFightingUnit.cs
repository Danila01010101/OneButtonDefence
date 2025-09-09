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
        gnomeSkinChanger = new GnomeSkinChanger(meshFilter, render, audioSource);
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

    public void ChangeSkin(SkinData data) => gnomeSkinChanger.ChangeSkin(data);
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class GnomeFightingUnit : FightingUnit
{
    [SerializeField] protected List<MeshFilter> meshFilter;
    [SerializeField] protected Transform modelOffsetTransform;
    
    private GnomeSkinChanger gnomeSkinChanger;

    public static Action GnomeDied;

    public override void Initialize(IEnemyDetector detector)
    {
        base.Initialize(detector);
        gnomeSkinChanger = new GnomeSkinChanger(meshFilter, render, modelOffsetTransform, audioSource);
    }

    protected override void Die()
    {
        GnomeDied?.Invoke();
        base.Die();
    }

    private void OnDestroy()
    {
        if (gnomeSkinChanger != null)
            gnomeSkinChanger.Unsubscribe();
        gnomeSkinChanger = null;
    }

    public void ChangeSkin(SkinData data)
    {
        currentDeathSound = data.DeathSound;
        gnomeSkinChanger.ChangeSkin(data);
    }
}
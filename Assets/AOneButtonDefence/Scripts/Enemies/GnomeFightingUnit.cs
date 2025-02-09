using UnityEngine;

public class GnomeFightingUnit : FightingUnit
{
    [SerializeField] protected MeshFilter meshFilter;
    
    private SkinChanger skinChanger;

    public override void Initialize()
    {
        base.Initialize();
        skinChanger = new SkinChanger(meshFilter, render);
    }

    public void ChangeSkin(Mesh newMesh, Material newMaterial) => skinChanger.ChangeSkin(newMesh, newMaterial);
}
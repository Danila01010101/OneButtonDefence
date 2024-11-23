using UnityEngine;

public class Ground : MonoBehaviour
{
    public GroundBlockType Type { get; protected set; } = GroundBlockType.Empty;
    public virtual void ActivateBonus() { }
    public enum GroundBlockType { Empty, Ordinary, MainBuilding, WaterBlock, CampBlock }

    public virtual void Initialize()
    {
        Type = GroundBlockType.Ordinary;
    }
}
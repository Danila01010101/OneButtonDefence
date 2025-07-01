using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/new Resource")]
public class ResourceData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public ResourceType Type { get; private set; }
    [field: SerializeField] public bool IsSpawnable { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }

    public enum ResourceType
    {
        Food, 
        Warrior,
        Spirit,
        Material,
        Gem,
        StrenghtBuff
    }
}
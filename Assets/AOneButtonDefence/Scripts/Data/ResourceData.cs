using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/new Resource")]
public class ResourceData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}
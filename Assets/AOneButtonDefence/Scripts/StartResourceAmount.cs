using UnityEngine;

[System.Serializable]
public class StartResourceAmount
{
    [field: SerializeField] public ResourceData Resource { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
}
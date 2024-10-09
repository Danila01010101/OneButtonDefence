using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "new Character Stats")]
public class CharacterStats : ScriptableObject
{
    public int Health { get; private set; } = 100;
    public int Damage { get; private set; } = 30;
    public float Speed { get; private set; } = 5;
    public float DetectionRadius { get; private set; } = 5;
    public LayerMask EnemyLayerMask { get; private set; }
}
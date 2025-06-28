using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/new Character Stats")]
public class CharacterStats : ScriptableObject
{
    [field : SerializeField] public int Health { get; private set; } = 100;
    [field : SerializeField] public int Damage { get; private set; } = 30;
    [field : SerializeField] public int DamageUpgradeValue { get; private set; } = 2;
    [field : SerializeField] public float Speed { get; private set; } = 5;
    [field : SerializeField] public float DetectionRadius { get; private set; } = 5;
    [field : SerializeField] public float ChaseRange { get; private set; } = 1;
    [field : SerializeField] public float AttackDelay { get; private set; } = 1;
    [field : SerializeField] public LayerMask EnemyLayerMask { get; private set; }
    [field : SerializeField] public float FadeDuration { get; private set; }
    [field : SerializeField] public float Delay { get; private set; }
    [field : SerializeField] public Color StartColor {  get; private set; }
    [field : SerializeField] public Color EndColor { get; private set; }
    [field : SerializeField] public int MinGemSpawn { get; private set; } = 1;
    [field : SerializeField] public int MaxGemSpawn { get; private set; } = 2;
    [field : SerializeField] public AudioClip DeathSound { get; private set; }
}
using UnityEngine;
using UnityEngine.Serialization;

namespace AOneButtonDefence.Scripts.Data
{
    [CreateAssetMenu(fileName = "PlayerControllerData", menuName = "ScriptableObjects/new PlayerControllerData", order = 1)]
    public class PlayerControllerData : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
        [field : SerializeField] public float RotationSpeed  { get; private set; } = 10f;
    }
}
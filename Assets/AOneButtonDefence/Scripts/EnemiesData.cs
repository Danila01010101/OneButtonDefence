using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesData", menuName = "ScriptableObjects/new Enemies Data")]
public class EnemiesData : ScriptableObject
{
    [SerializeField] public List<GameObject> enemies { get; private set; }
}
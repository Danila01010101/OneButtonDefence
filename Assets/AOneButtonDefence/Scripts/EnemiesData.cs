using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesData", menuName = "ScriptableObjects/new Enemies Data")]
public class EnemiesData : ScriptableObject
{
    [field : SerializeField] public List<Enemy> enemies { get; private set; }
}
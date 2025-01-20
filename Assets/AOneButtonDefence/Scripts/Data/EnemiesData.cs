using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesData", menuName = "ScriptableObjects/new Enemies Data")]
public class EnemiesData : ScriptableObject
{
    [field : SerializeField] public List<Knight> enemies { get; private set; }
}
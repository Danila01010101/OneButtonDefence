using System;
using UnityEngine;

public interface IEnemyDetector
{
    Transform GetClosestEnemy(Vector3 searchCenter);
    public event Action NewEnemiesDetected;
}

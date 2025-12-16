using System;
using UnityEngine;

public interface IEnemyDetector
{
    TargetToFollowInfo GetClosestEnemy(Vector3 searchCenter, float viewRadius);
    public event Action NewEnemiesDetected;
}

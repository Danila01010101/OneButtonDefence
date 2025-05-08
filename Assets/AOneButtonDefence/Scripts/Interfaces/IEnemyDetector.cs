using System;
using UnityEngine;

public interface IEnemyDetector
{
    TargetToFollowInfo GetClosestEnemy(Vector3 searchCenter);
    public event Action NewEnemiesDetected;
}

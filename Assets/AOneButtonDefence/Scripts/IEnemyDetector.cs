using UnityEngine;

public interface IEnemyDetector
{
    Transform GetClosestEnemy(Vector3 searchCenter);
}

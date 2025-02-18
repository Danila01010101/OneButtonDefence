using UnityEngine;

public class UnitDetector : MonoBehaviour, IEnemyDetector
{
    private Transform[] detectedEnemies;
    private float detectionInterval;
    private float lastCheckTime;
    private Vector3 detectRadius;
    private LayerMask layerMask;

    public UnitDetector(Vector3 detectBoxRadius, LayerMask enemyMask, float detectionInterval)
    {
        detectRadius = detectBoxRadius;
        layerMask = enemyMask;
        this.detectionInterval = detectionInterval;
    }

    public Transform GetClosestEnemy(Vector3 searchCenter)
    {
        float closestDistance = float.MaxValue;
        Transform closestTransform = null;

        foreach (Transform enemy in detectedEnemies)
        {
            IDamagable foundEnemy;

            if (enemy.gameObject.TryGetComponent(out foundEnemy))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);

                if (closestDistance > distanceToEnemy)
                {
                    closestDistance = distanceToEnemy;
                    closestTransform = enemy;
                }
            }
        }

        return closestTransform;
    }

    private Collider[] FindEnemies() => Physics.OverlapBox(transform.position, detectRadius, Quaternion.identity, layerMask);
}
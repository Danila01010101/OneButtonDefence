using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitDetector : IEnemyDetector
{
    private readonly List<Transform> detectedEnemies = new List<Transform>();
    private readonly float defaultStoppingDistance;
    private readonly float detectionInterval;
    private readonly Vector3 detectRadius;
    private readonly LayerMask layerMask;
    private readonly Vector3 worldCenter = new Vector3(155, -1, 155);

    private static readonly int MaxColliders = 1000;
    private readonly Collider[] colliderBuffer = new Collider[MaxColliders];

    public event Action NewEnemiesDetected;

    public UnitDetector(Vector3 detectBoxRadius, LayerMask enemyMask, float detectionInterval, float defaultStoppingDistance)
    {
        detectRadius = detectBoxRadius;
        layerMask = enemyMask;
        this.detectionInterval = detectionInterval;
        this.defaultStoppingDistance = defaultStoppingDistance;
        CoroutineStarter.Instance.StartCoroutine(EnemyDetection());
    }

    public TargetToFollowInfo GetClosestEnemy(Vector3 searchCenter)
    {
        float closestDistanceSqr = float.MaxValue;
        Transform closestTransform = null;
        NavMeshAgent foundAgent;

        foreach (Transform enemy in detectedEnemies)
        {
            if (enemy == null)
                continue;
            
            float distanceSqr = (searchCenter - enemy.position).sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestTransform = enemy;
            }
        }

        if (closestTransform != null && closestTransform.TryGetComponent(out foundAgent))
        {
            return new TargetToFollowInfo(closestTransform, foundAgent.radius);
        }
        else
        {
            return new TargetToFollowInfo(closestTransform, 0);
        }
    }
    
    private void FindEnemies()
    {
        int count = Physics.OverlapBoxNonAlloc(worldCenter, detectRadius, colliderBuffer, Quaternion.identity, layerMask);
        
        if (count != detectedEnemies.Count || HasEnemiesChanged(count))
        {
            detectedEnemies.Clear();
            
            for (int i = 0; i < count; i++)
            {
                detectedEnemies.Add(colliderBuffer[i].transform);
            }
            
            NewEnemiesDetected?.Invoke();
        }
    }

    private bool HasEnemiesChanged(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i >= detectedEnemies.Count || detectedEnemies[i] != colliderBuffer[i].transform)
            {
                return true;
            }
        }
        
        return false;
    }

    private IEnumerator EnemyDetection()
    {
        while (true)
        {
            yield return new WaitForSeconds(detectionInterval);
            FindEnemies();
        }
    }
}
using System;
using UnityEngine;
using WrightAngle.Waypoint;

public class Knight : FightingUnit
{
    [SerializeField] private WaypointTarget waypoint;
    
    public static Action<Vector3, int> OnEnemyDeath;
    private void Start()
    {
        if (waypoint == null)
            throw new NullReferenceException();
        
        waypoint.ActivateWaypoint();
        WaypointUIManager.OnWaypointTargetAdd.Invoke(waypoint);
    }
    protected override void Die()
    {
        base.Die();
        int gemsAmount = UnityEngine.Random.Range(characterStats.MinGemSpawn, characterStats.MaxGemSpawn);
        EnemyDeathManager.Instance.NotifyEnemyDeath(transform.position, gemsAmount);
        waypoint.DestroyWaypoint();
    }
}
using System;
using UnityEngine;
using WrightAngle.Waypoint;

[RequireComponent(typeof(WaypointTarget))]
public class Knight : FightingUnit
{
    public static Action<Vector3, int> OnEnemyDeath;
    private WaypointTarget waypoint;
    private void Start()
    {
        waypoint = GetComponent<WaypointTarget>();
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
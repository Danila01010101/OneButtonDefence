using UnityEngine;

public interface IEnemyDeathListener
{
    void OnEnemyDeath(Vector3 position, int revardAmount);
}
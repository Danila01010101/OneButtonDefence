using UnityEngine;

public interface ITargetFollower
{
    void SetTarget(Transform transform, float stoppingDistance);
}
using UnityEngine;

public class TargetToFollowInfo
{
    public readonly Transform Target;
    public readonly float TargetRadius;

    public TargetToFollowInfo(Transform target, float targetRadius)
    {
        Target = target;
        TargetRadius = targetRadius;
    }
}
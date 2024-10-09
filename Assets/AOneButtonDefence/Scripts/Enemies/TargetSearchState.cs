using System.Linq;
using UnityEngine;

public class TargetSearchState : IState
{
    private IStateChanger stateMachine;
    private Transform transform;
    private float detectRange;
    private LayerMask detectMask;

    public TargetSearchState(IStateChanger stateMachine, Transform transform, float detectRange, LayerMask detectMask)
    {
        this.stateMachine = stateMachine;
        this.transform = transform;
        this.detectRange = detectRange;
        this.detectMask = detectMask;
    }

    public void Enter() { }

    public void Exit() { }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate()
    {
        Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, detectRange, detectMask);

        if (detectedEnemies.Count() == 0)
            return;

        Transform detectedEnemy = ChooseEnemy(detectedEnemies);

        if (detectedEnemy != null)
            stateMachine.ChangeState<TargetFollowingState>();
    }

    public void Update() { }

    private Transform ChooseEnemy(Collider[] enemies)
    {
        float closestDistance = float.MaxValue;
        Transform closestTransform = null;

        foreach (Collider collider in enemies)
        {
            if (collider.gameObject.GetComponent<IDamagable>() != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.gameObject.transform.position);

                if (closestDistance > distanceToEnemy)
                {
                    closestDistance = distanceToEnemy;
                    closestTransform = collider.gameObject.transform;
                }
            }
        }

        return closestTransform;
    }
}
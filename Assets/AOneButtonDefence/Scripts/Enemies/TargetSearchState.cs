using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TargetSearchState : IState
{
    private IStateChanger stateMachine;
    private Transform transform;
    private float detectRange;
    private LayerMask detectMask;
    private ITargetFollower targetFollower;
    private NavMeshAgent agent;
    private Vector3 startPosition;
    private WalkingAnimation walkingAnimation;
    private bool isOnTheWay;

    public TargetSearchState(TargetSearchStateData data)
    {
        this.stateMachine = data.StateMachine;
        this.transform = data.Transform;
        this.detectRange = data.DetectRange;
        this.detectMask = data.DetectMask;
        this.targetFollower = data.TargetFollower;
        this.agent = data.Agent;
        this.startPosition = data.StartPosition;
        this.walkingAnimation = data.WalkingAnimation;
    }

    public void Enter() { }

    public void Exit()
    {
        agent.ResetPath();
        walkingAnimation.StopAnimation();
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate()
    {
        Collider[] enemies = FindEnemies();

        if (enemies.Length == 0 && !isOnTheWay)
        {
            GoToStartPosition();
            return;
        }
        
        Transform detectedEnemy = ChooseEnemy(enemies);

        if (detectedEnemy != null)
        {
            targetFollower.SetTarget(detectedEnemy);
            isOnTheWay = false;
            stateMachine.ChangeState<TargetFollowingState>();
        }
    }

    public void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && isOnTheWay)
        {
            walkingAnimation.StopAnimation();
            isOnTheWay = false;
        }
    }

    private void GoToStartPosition()
    {
        isOnTheWay = true;
        walkingAnimation.StartAnimation();
        agent.SetDestination(startPosition);
    }

    private bool IsEnemiesInRange() => FindEnemies().Count() > 0;

    private Collider[] FindEnemies() => Physics.OverlapSphere(transform.position, detectRange, detectMask);

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

    public class TargetSearchStateData
    {
        public IStateChanger StateMachine { get; private set; }
        public Transform Transform { get; private set; }
        public float DetectRange { get; private set; }
        public LayerMask DetectMask { get; private set; }
        public ITargetFollower TargetFollower { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public WalkingAnimation WalkingAnimation { get; private set; }

        public TargetSearchStateData(IStateChanger stateMachine, Transform transform, float detectRange, LayerMask detectMask,
            ITargetFollower targetFollower, NavMeshAgent agent, Vector3 startPosition, WalkingAnimation walkingAnimation)
        {
            StateMachine = stateMachine;
            Transform = transform;
            DetectRange = detectRange;
            DetectMask = detectMask;
            TargetFollower = targetFollower;
            Agent = agent;
            StartPosition = startPosition;
            WalkingAnimation = walkingAnimation;
        }
    }
}
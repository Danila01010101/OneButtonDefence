using UnityEngine;
using UnityEngine.AI;

public class TargetSearchState : IState
{
    private IStateChanger stateMachine;
    private WalkingAnimation walkingAnimation;
    private Transform transform;
    private float detectRange;
    private LayerMask detectMask;
    private ITargetFollower targetFollower;
    private NavMeshAgent agent;
	private bool isBattleGoing = false;
    private float enemyCheckInterval = 0.4f;
    private float lastTimeEnemyChecked;

    public TargetSearchState(TargetSearchStateData data)
    {
        stateMachine = data.StateMachine;
        transform = data.Transform;
        detectRange = data.DetectRange;
        detectMask = data.DetectMask;
        targetFollower = data.TargetFollower;
        agent = data.Agent;
        walkingAnimation = data.WalkingAnimation;
    }

    public void Enter()
    {
        Debug.Log("TargetSearch entered");
        lastTimeEnemyChecked = 0;
        isBattleGoing = true;
    }

    public void Exit()
    {
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
        if (Time.time - lastTimeEnemyChecked < enemyCheckInterval)
            return;

        Collider[] enemies = FindEnemies();
        
        if (enemies.Length == 0 && isBattleGoing == false)
        {
            stateMachine.ChangeState<IdleWarriorState>();
        }
        
        Transform detectedEnemy = ChooseEnemy(enemies);

        if (detectedEnemy != null)
        {
            targetFollower.SetTarget(detectedEnemy);
            stateMachine.ChangeState<TargetFollowingState>();
        }
    }

    public void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            walkingAnimation.StopAnimation();
        }
    }

    private Collider[] FindEnemies() => Physics.OverlapSphere(transform.position, detectRange, detectMask);

    private Transform ChooseEnemy(Collider[] enemies)
    {
        lastTimeEnemyChecked = Time.time;
        float closestDistance = float.MaxValue;
        Transform closestTransform = null;

        foreach (Collider collider in enemies)
        {
            IDamagable foundEnemy;
            
            if (collider.gameObject.TryGetComponent<IDamagable>(out foundEnemy))
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
        public WalkingAnimation WalkingAnimation { get; private set; }

        public TargetSearchStateData(IStateChanger stateMachine, Transform transform, float detectRange, LayerMask detectMask,
            ITargetFollower targetFollower, NavMeshAgent agent, WalkingAnimation walkingAnimation)
        {
            StateMachine = stateMachine;
            Transform = transform;
            DetectRange = detectRange;
            DetectMask = detectMask;
            TargetFollower = targetFollower;
            Agent = agent;
            WalkingAnimation = walkingAnimation;
        }
    }
}
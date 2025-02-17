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
    private float enemyCheckInterval = 0.4f;
    private float lastTimeEnemyChecked;
    private bool isBattleGoing = true;

    public TargetSearchState(TargetSearchStateData data)
    {
        stateMachine = data.StateMachine;
        transform = data.Transform;
        detectRange = data.DetectRange;
        detectMask = data.DetectMask;
        targetFollower = data.TargetFollower;
        agent = data.Agent;
        startPosition = data.StartPosition;
        walkingAnimation = data.WalkingAnimation;
        GameBattleState.BattleWon += DetectBattleEnd;
        GameBattleState.BattleLost += DetectBattleEnd;
        GameBattleState.BattleStarted += DetectBattleStart;
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
        
        if (isBattleGoing == false || Time.time - lastTimeEnemyChecked < enemyCheckInterval)
            return;
        
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
    
    private void DetectBattleEnd() => isBattleGoing = false;
    
    private void DetectBattleStart() => isBattleGoing = true;

    private bool IsEnemiesInRange() => FindEnemies().Count() > 0;

    private Collider[] FindEnemies() => Physics.OverlapSphere(transform.position, detectRange, detectMask);

    private Transform ChooseEnemy(Collider[] enemies)
    {
        Debug.Log("Enemy checked");
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
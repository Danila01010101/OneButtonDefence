using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TargetSearchState : UnitStateBase
{
    private readonly WalkingAnimation walkingAnimation;
    private readonly ITargetFollower targetFollower;
    private readonly IEnemyDetector detector;
    private readonly NavMeshAgent agent;
    private readonly Vector3 startPosition;
    private Coroutine coroutine;
    
    protected readonly float detectionRadius;

    public TargetSearchState(TargetSearchStateData data, CharacterStatsCounter statsCounter) 
        : base(data.StateMachine, data.SelfTransform, statsCounter)
    {
        targetFollower = data.TargetFollower;
        detector = data.Detector;
        walkingAnimation = data.WalkingAnimation;
        agent = data.Agent;
        detectionRadius = data.DetectionRadius;
        startPosition = data.StartPosition;
    }

    public override void Enter()
    {
        coroutine = CoroutineStarter.Instance.StartCoroutine(EnemyDetection());
        detector.NewEnemiesDetected += LookForTarget;
    }

    public override void Exit()
    {
        if (coroutine != null)
            CoroutineStarter.Instance.StopCoroutine(coroutine);
        
        walkingAnimation.StopAnimation();
        detector.NewEnemiesDetected -= LookForTarget;
    }

    public override void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            walkingAnimation.StopAnimation();
        }
        
        if (!BattleNotifier.Instance.IsBattleGoing())
        {
            StateMachine.ChangeState<IdleWarriorState>();
        }
    }

    protected virtual void LookForTarget()
    {
        if (SelfTransform == null)
            return;
            
        var enemyInfo = detector.GetClosestEnemy(SelfTransform.position, detectionRadius);

        if (enemyInfo.Target == null)
            return;

        if (Vector3.Distance(SelfTransform.position, enemyInfo.Target.transform.position) <= detectionRadius)
        {
            targetFollower.SetTarget(enemyInfo.Target, enemyInfo.TargetRadius);
            StateMachine.ChangeState<TargetFollowingState>();
        }
        else
        {
            walkingAnimation.StartAnimation();
            agent.SetDestination(startPosition);
        }
    }

    private IEnumerator EnemyDetection()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            LookForTarget();
        }   
    }

    public class TargetSearchStateData
    {
        public IStateChanger StateMachine { get; }
        public Transform SelfTransform { get; }
        public ITargetFollower TargetFollower { get; }
        public NavMeshAgent Agent { get; }
        public WalkingAnimation WalkingAnimation { get; }
        public IEnemyDetector Detector { get; }
        public float DetectionRadius { get; }
        public Vector3 StartPosition { get; }

        public TargetSearchStateData(IStateChanger stateMachine, Transform selfTransform, ITargetFollower targetFollower,
            NavMeshAgent agent, WalkingAnimation walkingAnimation, IEnemyDetector detector, float detectionRadius, Vector3 startPosition)
        {
            StateMachine = stateMachine;
            SelfTransform = selfTransform;
            TargetFollower = targetFollower;
            Agent = agent;
            WalkingAnimation = walkingAnimation;
            Detector = detector;
            DetectionRadius = detectionRadius;
            StartPosition = startPosition;
        }
    }
}
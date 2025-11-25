using UnityEngine;
using UnityEngine.AI;

public class TargetSearchState : UnitStateBase
{
    private readonly WalkingAnimation walkingAnimation;
    private readonly ITargetFollower targetFollower;
    private readonly IEnemyDetector detector;
    private readonly NavMeshAgent agent;

    public TargetSearchState(TargetSearchStateData data, bool isPlayerControlled, CharacterStatsCounter statsCounter) 
        : base(data.StateMachine, data.SelfTransform, statsCounter, isPlayerControlled)
    {
        targetFollower = data.TargetFollower;
        detector = data.Detector;
        walkingAnimation = data.WalkingAnimation;
        agent = data.Agent;
    }

    public override void Enter()
    {
        LookForTarget();
        detector.NewEnemiesDetected += LookForTarget;
    }

    public override void Exit()
    {
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
            
        var enemyInfo = detector.GetClosestEnemy(SelfTransform.position);

        if (enemyInfo.Target == null)
            return;
        
        targetFollower.SetTarget(enemyInfo.Target, enemyInfo.TargetRadius);
        StateMachine.ChangeState<TargetFollowingState>();
    }

    public class TargetSearchStateData
    {
        public IStateChanger StateMachine { get; }
        public Transform SelfTransform { get; }
        public ITargetFollower TargetFollower { get; }
        public NavMeshAgent Agent { get; }
        public WalkingAnimation WalkingAnimation { get; }
        public IEnemyDetector Detector { get; }

        public TargetSearchStateData(IStateChanger stateMachine, Transform selfTransform, ITargetFollower targetFollower,
            NavMeshAgent agent, WalkingAnimation walkingAnimation, IEnemyDetector detector)
        {
            StateMachine = stateMachine;
            SelfTransform = selfTransform;
            TargetFollower = targetFollower;
            Agent = agent;
            WalkingAnimation = walkingAnimation;
            Detector = detector;
        }
    }
}
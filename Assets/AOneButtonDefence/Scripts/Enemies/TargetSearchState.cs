using UnityEngine;
using UnityEngine.AI;

public class TargetSearchState : IState
{
    private readonly IStateChanger stateMachine;
    private readonly WalkingAnimation walkingAnimation;
    private readonly ITargetFollower targetFollower;
    private readonly Transform transform;
    private readonly IEnemyDetector detector;
    private readonly NavMeshAgent agent;

    public TargetSearchState(TargetSearchStateData data)
    {
        stateMachine = data.StateMachine;
        targetFollower = data.TargetFollower;
        agent = data.Agent;
        transform = data.SelfTransform;
        detector = data.Detector;
        walkingAnimation = data.WalkingAnimation;
    }

    public virtual void Enter()
    {
        LookForTarget();
        detector.NewEnemiesDetected += LookForTarget;
    }

    public virtual void Exit()
    {
        walkingAnimation.StopAnimation();
        detector.NewEnemiesDetected -= LookForTarget;
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            walkingAnimation.StopAnimation();
        }
        
        if (BattleNotifier.Instance.IsBattleGoing() == false)
        {
            stateMachine.ChangeState<IdleWarriorState>();
        }
    }

    protected virtual void LookForTarget()
    {
        Transform detectedEnemy = null;
        float detectedEnemyStoppingDistance = 0f;
        
        if (transform != null)
        {
            var enemyInfo = detector.GetClosestEnemy(transform.position);
            detectedEnemy = enemyInfo.Target;
            detectedEnemyStoppingDistance = enemyInfo.TargetRadius;
        }
        else
        {
            Exit();
        }
        
        if (detectedEnemy == null)
            return;
        
        targetFollower.SetTarget(detectedEnemy, detectedEnemyStoppingDistance);
        stateMachine.ChangeState<TargetFollowingState>();
    }
    
    public class TargetSearchStateData
    {
        public IStateChanger StateMachine { get; private set; }
        public Transform SelfTransform { get; private set; }
        public ITargetFollower TargetFollower { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public WalkingAnimation WalkingAnimation { get; private set; }

        public IEnemyDetector Detector { get; private set; }

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
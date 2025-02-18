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
        transform = data.Transform;
        detector = data.Detector;
        walkingAnimation = data.WalkingAnimation;
    }

    public void Enter()
    {
        LookForTarget();
        detector.NewEnemiesDetected += LookForTarget;
    }

    public void Exit()
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

    private void LookForTarget()
    {
        var detectedEnemy = detector.GetClosestEnemy(transform.position);
        
        if (detectedEnemy == null)
            return;
        
        targetFollower.SetTarget(detectedEnemy);
        stateMachine.ChangeState<TargetFollowingState>();
    }
    
    public class TargetSearchStateData
    {
        public IStateChanger StateMachine { get; private set; }
        public Transform Transform { get; private set; }
        public ITargetFollower TargetFollower { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public WalkingAnimation WalkingAnimation { get; private set; }

        public IEnemyDetector Detector { get; private set; }

        public TargetSearchStateData(IStateChanger stateMachine, Transform transform,
            ITargetFollower targetFollower, NavMeshAgent agent, WalkingAnimation walkingAnimation, IEnemyDetector detector)
        {
            StateMachine = stateMachine;
            Transform = transform;
            TargetFollower = targetFollower;
            Agent = agent;
            WalkingAnimation = walkingAnimation;
            Detector = detector;
        }
    }
}
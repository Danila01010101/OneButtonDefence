using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace AOneButtonDefence.Scripts
{
    public abstract class UnitStateMachineDataBaseClass
    {
        public readonly Transform SelfTransform;
        public readonly NavMeshAgent Agent;
        public readonly WalkingAnimation WalkingAnimation;
        public readonly IAttackAnimator FightAnimation;
        public readonly IEnemyDetector EnemyDetector;
        public readonly ISelfDamageable SelfDamageable;
        
        public UnitStateMachineDataBaseClass(Transform selfTransform, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, IAttackAnimator fightAnimation, IEnemyDetector detector, ISelfDamageable selfDamageable)
        {
            SelfTransform = selfTransform;
            Agent = agent;
            WalkingAnimation = walkingAnimation;
            FightAnimation = fightAnimation;
            EnemyDetector = detector;
            SelfDamageable = selfDamageable;
        }
    }
}
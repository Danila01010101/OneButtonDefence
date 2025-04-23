using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace AOneButtonDefence.Scripts
{
    public abstract class UnitStateMachineDataBaseClass
    {
        public readonly Transform Transform;
        public readonly NavMeshAgent Agent;
        public readonly WalkingAnimation WalkingAnimation;
        public readonly IAttackAnimator FightAnimation;
        public readonly IEnemyDetector EnemyDetector;
        
        public UnitStateMachineDataBaseClass(Transform transform, NavMeshAgent agent, 
            WalkingAnimation walkingAnimation, IAttackAnimator fightAnimation, IEnemyDetector detector)
        {
            Transform = transform;
            Agent = agent;
            WalkingAnimation = walkingAnimation;
            FightAnimation = fightAnimation;
            EnemyDetector = detector;
        }
    }
}
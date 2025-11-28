using System;
using UnityEngine;

public class BossFightBattleState : IState
{
    private IStringStateChanger stateMachine;
    private string enemyTag;
    private string gnomeTag;
    
    public static Action BattleStarted;
    public static Action EnemiesDefeated;
    
    public void Enter()
    {
        BattleStarted?.Invoke();
    }

    public void Exit()
    {
        
    }

    public void HandleInput()
    {
        
    }

    public void Update()
    {
        
    }

    public void PhysicsUpdate() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }
}

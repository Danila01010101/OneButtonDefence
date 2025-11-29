using System;
using UnityEngine;

public class BossFightBattleState : IState
{
    private IStringStateChanger stateMachine;
    private string enemyTag;
    private string gnomeTag;
    private GameObject battleCanvas;
    
    public static Action BattleStarted;
    public static Action EnemiesDefeated;

    public BossFightBattleState(GameObject battleCanvas)
    {
        this.battleCanvas = battleCanvas;
    }
    
    public void Enter()
    {
        BattleStarted?.Invoke();
        battleCanvas.gameObject.SetActive(true);
    }

    public void Exit()
    {
        battleCanvas.gameObject.SetActive(false);
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

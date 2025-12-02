using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightBattleState : IState
{
    private IStringStateChanger stateMachine;
    private string enemyTag;
    private string gnomeTag;
    private GameObject battleCanvas;
    private UnitsFactory unitsFactory;
    
    public static Action BattleStarted;
    public static Action EnemiesDefeated;

    public BossFightBattleState(BossFightBattleStateData battleStateData)
    {
        battleCanvas = battleStateData.BattleCanvas;
        unitsFactory = new UnitsFactory(battleStateData.EnemiesData.enemies, battleStateData.Detector, battleStateData.EnemyLayer, battleStateData.EnemyTag);
        unitsFactory.SpawnUnit<DragonUnit>(battleStateData.DragonSpawnPosition);
    }
    
    public void Enter()
    {
        CoroutineStarter.Instance.StartCoroutine(NotifyBattleStartedWithDelay());
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

    private IEnumerator NotifyBattleStartedWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        BattleStarted?.Invoke();
    }
}

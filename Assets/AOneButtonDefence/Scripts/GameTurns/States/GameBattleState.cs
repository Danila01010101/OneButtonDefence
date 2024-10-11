using System.Collections;
using UnityEngine;

public class GameBattleState : IState
{
    private IStateChanger stateMachine;
    private MonoBehaviour coroutineStarter;
    private BattleWavesParameters wavesParameters;

    public GameBattleState(IStateChanger stateMachine, MonoBehaviour coroutineStarter, BattleWavesParameters wavesParameters)
    {
        this.stateMachine = stateMachine;
        this.coroutineStarter = coroutineStarter;
        this.wavesParameters = wavesParameters;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void HandleInput()
    {
        throw new System.NotImplementedException();
    }

    public void OnAnimationEnterEvent()
    {
        throw new System.NotImplementedException();
    }

    public void OnAnimationExitEvent()
    {
        throw new System.NotImplementedException();
    }

    public void OnAnimationTransitionEvent()
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerEnter(Collider collider)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerExit(Collider collider)
    {
        throw new System.NotImplementedException();
    }

    public void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }

    private void StartWave()
    {
        coroutineStarter.StartCoroutine(StartEnemiesSpawn());
    }

    private IEnumerator StartEnemiesSpawn()
    {
        yield return null;
    }
}
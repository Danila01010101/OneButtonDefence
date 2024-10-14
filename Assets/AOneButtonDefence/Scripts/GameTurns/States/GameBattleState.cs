using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameBattleState : IState
{
    private IStateChanger stateMachine;
    private MonoBehaviour coroutineStarter;
    private BattleWavesParameters wavesParameters;
    private EnemieFactory enemieFactory;
    private Coroutine spawnCoroutine;

    public GameBattleState(IStateChanger stateMachine, MonoBehaviour coroutineStarter, BattleWavesParameters wavesParameters, EnemiesData data)
    {
        this.stateMachine = stateMachine;
        this.coroutineStarter = coroutineStarter;
        this.wavesParameters = wavesParameters;
        enemieFactory = new EnemieFactory(data);
    }

    public void Enter() { }

    public void Exit()
    {
        if (spawnCoroutine != null)
        {
            coroutineStarter.StopCoroutine(spawnCoroutine);
        }
    }

    public void HandleInput() { }

    public void OnAnimationEnterEvent() { }

    public void OnAnimationExitEvent() { }

    public void OnAnimationTransitionEvent() { }

    public void OnTriggerEnter(Collider collider) { }

    public void OnTriggerExit(Collider collider) { }

    public void PhysicsUpdate() { }

    public void Update() { }

    private void StartWave()
    {
        spawnCoroutine = coroutineStarter.StartCoroutine(StartEnemiesSpawn());
    }

    private IEnumerator StartEnemiesSpawn()
    {
        yield return null;
    }
}
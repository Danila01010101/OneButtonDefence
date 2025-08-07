using System.Collections;
using UnityEngine;

public class ResourceChangeMediatorInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private GameData gameData;
    public static ResourceChangeMediator Mediator { get; private set; }

    public IEnumerator Initialize()
    {
        Mediator = new ResourceChangeMediator(gameData.GnomeDeathSpiritFine, gameData.GemsResource, gameData.GnomeDeathWarriorFine);
        Mediator.Subscribe();
        yield return null;
    }

    private void OnDestroy()
    {
        Mediator?.Unsubscribe();
    }
}
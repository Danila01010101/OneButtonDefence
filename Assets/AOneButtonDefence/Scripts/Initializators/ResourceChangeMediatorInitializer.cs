using System;
using System.Collections;

public class ResourceChangeMediatorInitializer : IGameInitializerStep, IDisposable
{
    private GameData _gameData;
    public ResourceChangeMediator Mediator { get; private set; }

    public ResourceChangeMediatorInitializer(GameData data)
    {
        _gameData = data;
    }

    public IEnumerator Initialize()
    {
        Mediator = new ResourceChangeMediator(_gameData.GnomeDeathSpiritFine, _gameData.GemsResource, _gameData.GnomeDeathWarriorFine);
        Mediator.Subscribe();
        yield break;
    }

    public void Dispose()
    {
        Mediator.Unsubscribe();
    }
}
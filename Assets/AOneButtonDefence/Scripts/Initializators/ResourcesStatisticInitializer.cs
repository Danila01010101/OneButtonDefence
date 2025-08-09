using System.Collections;
using System.Collections.Generic;

public class ResourcesStatisticInitializer : IGameInitializerStep
{
    private GameData _gameData;
    private GameResourcesCounter _counter;
    private UnitsFactory _gnomeFactory;
    public IncomeDifferenceTextConverter IncomeConverter { get; private set; }

    public ResourcesStatisticInitializer(GameData data, GameResourcesCounter counter, UnitsFactory gnomeFactory)
    {
        _gameData = data;
        _counter = counter;
        _gnomeFactory = gnomeFactory;
    }

    public IEnumerator Initialize()
    {
        var resources = new List<ResourceAmount>();
        foreach (var resource in _gameData.StartResources)
            resources.Add(new ResourceAmount(resource.Resource, 0));

        var resourceEffectsDictionary = new Dictionary<ResourceData.ResourceType, IResourceEffect>()
        {
            { ResourceData.ResourceType.Warrior, new WarriorResourceEffect(_gnomeFactory, _gameData.GnomeSpawnOffset) }
        };

        new ResourceIncomeCounter(_counter, resources, resourceEffectsDictionary);
        IncomeConverter = new IncomeDifferenceTextConverter();
        yield break;
    }
}
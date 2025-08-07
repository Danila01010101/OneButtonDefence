using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystemInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private GameData gameData;
    public static GameResourcesCounter GameResourcesCounter { get; private set; }
    public static IncomeDifferenceTextConverter IncomeConverter { get; private set; }

    public IEnumerator Initialize()
    {
        var counter = new GameObject("ResourcesCounter").AddComponent<GameResourcesCounter>();
        counter.transform.SetParent(transform);

        var resources = new List<ResourceAmount>();
        foreach (var resource in gameData.StartResources)
            resources.Add(new ResourceAmount(resource));

        counter.Initialize(resources);
        GameResourcesCounter = counter;

        var zeroedResources = new List<ResourceAmount>();
        foreach (var resource in gameData.StartResources)
            zeroedResources.Add(new ResourceAmount(resource.Resource, 0));

        var gnomeDetector = new UnitDetector(gameData.WorldSize, LayerMask.GetMask(gameData.EnemyLayerName), 1f, gameData.DefaultStoppingDistance);
        var gnomeFactory = new UnitsFactory(new List<FightingUnit> { gameData.GnomeUnit }, gnomeDetector, LayerMask.GetMask(gameData.GnomeLayerName), gameData.GnomeTag);

        var effectDict = new Dictionary<ResourceData.ResourceType, IResourceEffect>
        {
            { ResourceData.ResourceType.Warrior, new WarriorResourceEffect(gnomeFactory, gameData.GnomeSpawnOffset) }
        };

        new ResourceIncomeCounter(counter, zeroedResources, effectDict);
        IncomeConverter = new IncomeDifferenceTextConverter();

        yield return null;
    }

    private void OnDestroy()
    {
        if (ResourceIncomeCounter.Instance != null)
            ResourceIncomeCounter.Instance.Unsubscribe();
        IncomeConverter?.Unsubscribe();
    }
}
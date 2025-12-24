using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomBuildingsSelector
{
    private List<BasicBuildingData> allBuildings;
    private List<BasicBuildingData> randomizableBuildings;
    private List<BasicBuildingData> nonRandomizableBuildings;

    public RandomBuildingsSelector(List<BasicBuildingData> buildings)
    {
        allBuildings = buildings;
        nonRandomizableBuildings = allBuildings.Where(b => !b.IsRandomisable).ToList();
        randomizableBuildings = allBuildings.Where(b => b.IsRandomisable).ToList();
    }

    public List<BasicBuildingData> GetSelection(int count)
    {
        if (count <= 0)
            return new List<BasicBuildingData>();

        var result = new List<BasicBuildingData>();

        if (nonRandomizableBuildings.Count > 0)
        {
            var weightedNonRandom = GetWeightedSelection(nonRandomizableBuildings, 1);
            if (weightedNonRandom.Count > 0)
                result.Add(weightedNonRandom[0]);
        }

        if (result.Count < count)
        {
            var allAvailable = new List<BasicBuildingData>();
            allAvailable.AddRange(nonRandomizableBuildings.Where(b => !result.Contains(b)));
            allAvailable.AddRange(randomizableBuildings);

            int remainingSlots = count - result.Count;
            var weightedRandom = GetWeightedSelection(allAvailable, remainingSlots);
            result.AddRange(weightedRandom);
        }

        return ShuffleList(result);
    }

    private List<BasicBuildingData> GetWeightedSelection(List<BasicBuildingData> buildings, int count)
    {
        if (buildings.Count == 0 || count <= 0)
            return new List<BasicBuildingData>();

        var result = new List<BasicBuildingData>();
        var availableBuildings = new List<BasicBuildingData>(buildings);

        for (int i = 0; i < count && availableBuildings.Count > 0; i++)
        {
            float totalWeight = 0f;
            
            foreach (var building in availableBuildings)
            {
                totalWeight += GetSpawnWeight(building);
            }

            float randomPoint = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var building in availableBuildings)
            {
                currentWeight += GetSpawnWeight(building);
                if (currentWeight >= randomPoint)
                {
                    result.Add(building);
                    availableBuildings.Remove(building);
                    break;
                }
            }
        }

        return result;
    }

    private float GetSpawnWeight(BasicBuildingData building)
    {
        return building.SpawnChance;
    }

    private List<BasicBuildingData> ShuffleList(List<BasicBuildingData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }
}
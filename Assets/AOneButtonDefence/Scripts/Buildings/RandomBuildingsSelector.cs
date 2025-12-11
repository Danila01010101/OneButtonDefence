using System.Collections.Generic;

public class RandomBuildingsSelector
{
    private readonly List<BasicBuildingData> allBuildings;

    public RandomBuildingsSelector(List<BasicBuildingData> buildings)
    {
        allBuildings = buildings;
    }

    public List<BasicBuildingData> GetSelection(int count)
    {
        List<BasicBuildingData> pool = new List<BasicBuildingData>();

        foreach (var building in allBuildings)
        {
            if (!building.IsRandomisable || building.SpawnChance <= 0f)
                continue;

            pool.Add(building);
        }

        List<BasicBuildingData> result = new List<BasicBuildingData>();

        for (int i = 0; i < count; i++)
        {
            if (pool.Count == 0)
                break;

            var chosen = GetRandomWeighted(pool);
            result.Add(chosen);

            pool.Remove(chosen);
        }

        return result;
    }

    private BasicBuildingData GetRandomWeighted(List<BasicBuildingData> pool)
    {
        float totalWeight = 0f;
        foreach (var b in pool)
            totalWeight += b.SpawnChance;

        float rand = UnityEngine.Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var b in pool)
        {
            cumulative += b.SpawnChance;
            if (rand <= cumulative)
                return b;
        }

        return pool[pool.Count - 1];
    }
}
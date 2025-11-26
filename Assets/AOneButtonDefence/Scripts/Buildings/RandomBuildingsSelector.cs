using System.Collections.Generic;
using System.Linq;

public class RandomBuildingsSelector
{
    private readonly List<BasicBuildingData> fixedBuildings;
    private readonly List<BasicBuildingData> randomBuildings;

    public RandomBuildingsSelector(IEnumerable<BasicBuildingData> all)
    {
        fixedBuildings = all.Where(b => !b.IsRandomisable).ToList();
        randomBuildings = all.Where(b => b.IsRandomisable).ToList();
    }

    public List<BasicBuildingData> GetSelection(int randomCount)
    {
        var result = new List<BasicBuildingData>(fixedBuildings);

        if (randomBuildings.Count <= randomCount)
        {
            result.AddRange(randomBuildings);
            return result;
        }

        var selected = new HashSet<BasicBuildingData>();

        while (selected.Count < randomCount)
        {
            var r = randomBuildings[UnityEngine.Random.Range(0, randomBuildings.Count)];
            selected.Add(r);
        }

        result.AddRange(selected);
        return result;
    }
}
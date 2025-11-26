using System.Collections.Generic;
using UnityEngine;

public class BuildingButtonsSpawner
{
    private readonly UIInfoButton prefab;
    private readonly Transform parent;
    private readonly int distance;

    public BuildingButtonsSpawner(UIInfoButton prefab, Transform parent, int distance)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.distance = distance;
    }

    public List<UIInfoButton> Spawn(int count)
    {
        var list = new List<UIInfoButton>();

        if (count % 2 == 0)
        {
            int offset = 50;
            int half = count / 2;

            for (int i = 0; i < half; i++)
            {
                list.Add(SpawnOne(offset));
                offset += distance / 2;
                list.Add(SpawnOne(-offset));
                offset += distance / 2;
            }
        }
        else
        {
            list.Add(SpawnOne(0));

            int half = (count - 1) / 2;
            int offset = distance;

            for (int i = 0; i < half; i++)
            {
                list.Add(SpawnOne(offset));
                list.Add(SpawnOne(-offset));
                offset += distance;
            }
        }

        list.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        return list;
    }

    private UIInfoButton SpawnOne(int xOffset)
    {
        var btn = Object.Instantiate(prefab, parent);

        var rect = btn.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(xOffset, 0f);

        return btn;
    }
}
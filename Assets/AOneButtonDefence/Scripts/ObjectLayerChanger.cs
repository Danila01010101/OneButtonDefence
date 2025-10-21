using System.Collections.Generic;
using UnityEngine;

public static class ObjectLayerChanger
{
    public static void SetLayerRecursive(Transform root, int layer)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();
            current.gameObject.layer = layer;

            foreach (Transform child in current)
            {
                queue.Enqueue(child);
            }
        }
    }
    
    public static void SetLayerRecursive(Transform root, LayerMask layerMask)
    {
        int layer = GetSingleLayerFromMask(layerMask);
        SetLayerRecursive(root, layer);
    }

    private static int GetSingleLayerFromMask(LayerMask mask)
    {
        int value = mask.value;
        if (value == 0 || (value & (value - 1)) != 0)
        {
            throw new System.ArgumentException($"LayerMask '{mask}' содержит несколько слоёв. Метод поддерживает только один слой.");
        }

        return Mathf.RoundToInt(Mathf.Log(value, 2));
    }
}
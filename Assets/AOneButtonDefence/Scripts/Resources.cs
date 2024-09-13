using UnityEngine;

public class Resources : MonoBehaviour
{
	public static Resources Instance;

    public ResourcesData Data { get; private set; }

    // должен быть этот скрипт в сцене, но ТОЛЬКО один
    // для изменения из другого скрипта пишем Resources.Instance.Data += x; для изменения наших характеристик

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }


    public class ResourcesData
    {
        public int FoodAmount { get; set; }

        //остальная нужная статистика
    }
}
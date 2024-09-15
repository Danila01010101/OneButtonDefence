using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
	public static ResourcesCounter Instance;

    public ResourcesData Data { get; private set; }

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
        public int Warriors { get; set; }
        public int Materials { get; set; }
        public int SurvivorSpirit { get; set; }
    }
}
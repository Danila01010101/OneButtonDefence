using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private int humanAmount = 1;

    private ResourcesCounter.ResourcesData resources;

    private void Start()
    {
        resources = ResourcesCounter.Instance.Data;
    }

    public virtual void ActivateSpawnBonus()
    {
        
    }

    public virtual void AddBonus()
    {
        resources.FoodAmount -= humanAmount;
    }
}
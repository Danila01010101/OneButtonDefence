using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class PartManager : MonoBehaviour
{
    [SerializeField] private Button partPrefab;
    [SerializeField] private GameObject CellsSpawnParent;
    [SerializeField] private UpgradeButton upgradeButton;

    private int partPlacingInterval = 0;
    private float startButtonsAmount;
    private List<Button> parts = new List<Button>();
    private Building.BuildingType firstUpgrade = Building.BuildingType.Empty;
    private Building.BuildingType secondUpgrade = Building.BuildingType.Empty;
    private int howManyChois = 0;

    public UpgradeButton UpgradeButton => upgradeButton;

    public void Initialize(int partsAmount)
    {
        if (partsAmount % 2 == 0)
        {
            partPlacingInterval = 50;
            startButtonsAmount = partsAmount / 2;

            for (int i = 0; i < startButtonsAmount; i++)
            {
                var spa = Instantiate(partPrefab, CellsSpawnParent.transform.position + new Vector3(partPlacingInterval, 0, 0), Quaternion.identity);
                spa.transform.SetParent(CellsSpawnParent.transform);
                parts.Add(spa);
                var sp = Instantiate(partPrefab, CellsSpawnParent.transform.position + new Vector3(-partPlacingInterval, 0, 0), Quaternion.identity);
                sp.transform.SetParent(CellsSpawnParent.transform);
                parts.Add(sp);
                partPlacingInterval = partPlacingInterval + 100;
            }
        }
        else
        {
            startButtonsAmount = partsAmount / 2 + 0.5f;

            for (int i = 0; i < startButtonsAmount; i++)
            {
                var spawn = Instantiate(partPrefab, CellsSpawnParent.transform.position + new Vector3(partPlacingInterval, 0, 0), Quaternion.identity);
                spawn.transform.SetParent(CellsSpawnParent.transform);
                parts.Add(spawn);

                if (partPlacingInterval != 0)
                {
                    var spaw = Instantiate(partPrefab, CellsSpawnParent.transform.position + new Vector3(-partPlacingInterval, 0, 0), Quaternion.identity);
                    spaw.transform.SetParent(CellsSpawnParent.transform);
                    parts.Add(spaw);
                }

                partPlacingInterval = partPlacingInterval + 100;
            }
        }

        parts = parts.OrderBy(part => part.transform.position.x).ToList();
        
        for (int i = 0; i < parts.Count; i++)
        {
            Building.BuildingType buildingType = ConvertIntToUpgrade(i);
            parts[i].onClick.AddListener(delegate { ChoosePart(buildingType); });
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && parts.Count >= 1)
        {
            ChoosePart(Building.BuildingType.Factory);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && parts.Count >= 2)
        {
            ChoosePart(Building.BuildingType.Farm);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && parts.Count >= 3)
        {
            ChoosePart(Building.BuildingType.MilitaryCamp);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && parts.Count >= 4)
        {
            ChoosePart(Building.BuildingType.SpiritBuilding);
        }
    }

    private void ChoosePart(Building.BuildingType type)
    {
        howManyChois++;

        if(firstUpgrade != Building.BuildingType.Empty && secondUpgrade != Building.BuildingType.Empty)
        {
            if (secondUpgrade == type || firstUpgrade == type)
            {
                Debug.Log("Part already chosen or value is incorrect");
                return;
            }
        }

        if (howManyChois > 2)
        {
            parts[ConvertUpgradeToInt(type)].transform.GetChild(0).gameObject.SetActive(true);
            parts[ConvertUpgradeToInt(secondUpgrade)].transform.GetChild(0).gameObject.SetActive(false);
            secondUpgrade = Building.BuildingType.Empty;
            secondUpgrade = firstUpgrade;
            firstUpgrade = type;
        }
        else
        {
            parts[ConvertUpgradeToInt(type)].transform.GetChild(0).gameObject.SetActive(true);

            if (firstUpgrade != Building.BuildingType.Empty)
            {
                secondUpgrade = firstUpgrade;
            }

            firstUpgrade = type;
        }
    }

    public void WhenButtonClicked()
    {
        if (firstUpgrade == Building.BuildingType.Empty || secondUpgrade == Building.BuildingType.Empty)
        {
            Debug.Log("No upgrades choosen");
            return;
        }

        upgradeButton.UbgraidChoisenPart(firstUpgrade, secondUpgrade);
    }
}

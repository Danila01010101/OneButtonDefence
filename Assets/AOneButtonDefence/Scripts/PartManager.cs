using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PartManager : MonoBehaviour
{
    [SerializeField] private Button partPrefab;
    [SerializeField] private GameObject CellsSpawnParent;
    [SerializeField] private UpgradeButton upgradeButton;

    private int partPlacingInterval = 0;
    private float startButtonsAmount;
    private List<Button> parts = new List<Button>();
    private int lastKey = -1;
    private int beforLastKey = -1;
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
            int partIndex = i;
            parts[i].onClick.AddListener(delegate { ChoosePart(partIndex); });
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && parts.Count >= 1)
        {
            ChoosePart(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && parts.Count >= 2)
        {
            ChoosePart(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && parts.Count >= 3)
        {
            ChoosePart(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && parts.Count >= 4)
        {
            ChoosePart(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && parts.Count >= 5)
        {
            ChoosePart(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && parts.Count >= 6)
        {
            ChoosePart(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7) && parts.Count >= 7)
        {
            ChoosePart(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8) && parts.Count >= 8)
        {
            ChoosePart(7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9) && parts.Count >= 9)
        {
            ChoosePart(8);
        }
    }

    private void ChoosePart(int index)
    {
        howManyChois++;

        if(lastKey >= 0 && beforLastKey >= 0)
        {
            if (beforLastKey == index || lastKey == index)
            {
                Debug.Log("Part already chosen or value is incorrect");
                return;
            }
        }

        if (howManyChois > 2)
        {
            parts[index].transform.GetChild(0).gameObject.SetActive(true);
            parts[beforLastKey].transform.GetChild(0).gameObject.SetActive(false);
            beforLastKey = -1;
            beforLastKey = lastKey;
            lastKey = index;
        }
        else
        {
            parts[index].transform.GetChild(0).gameObject.SetActive(true);

            if (lastKey != -1)
            {
                beforLastKey = lastKey;
            }

            lastKey = index;
        }
    }

    public void WhenButtonClicked()
    {
        if (lastKey == -1 || beforLastKey == -1)
        {
            Debug.Log("No upgrades choosen");
            return;
        }

        upgradeButton.UbgraidChoisenPart((UpgradeButton.Upgrades) lastKey, (UpgradeButton.Upgrades) beforLastKey);
    }
}

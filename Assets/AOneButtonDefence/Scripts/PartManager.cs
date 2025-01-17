using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartManager : MonoBehaviour
{
    [SerializeField] private Button partPrefab;
    [SerializeField] private GameObject cellsSpawnParent;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private int buttonsDistance = 100;

    private int partPlacingInterval = 0;
    private float startButtonsAmount;
    private List<Button> parts = new List<Button>();
    private List<ButtonChooseAnimation> partsAnimators = new List<ButtonChooseAnimation>();
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
                SpawnButton(partPlacingInterval);
                partPlacingInterval = partPlacingInterval + buttonsDistance / 2;
                
                SpawnButton(-partPlacingInterval);
                partPlacingInterval = partPlacingInterval + buttonsDistance / 2;
            }
        }
        else
        {
            startButtonsAmount = partsAmount / 2 + 0.5f;

            for (int i = 0; i < startButtonsAmount; i++)
            {
                SpawnButton(partPlacingInterval);
                
                partPlacingInterval = partPlacingInterval + buttonsDistance / 2;

                if (partPlacingInterval != 0)
                {
                    SpawnButton(-partPlacingInterval);
                }

                partPlacingInterval = partPlacingInterval + buttonsDistance / 2;
            }
        }

        parts = parts.OrderBy(part => part.transform.position.x).ToList();
        partsAnimators = partsAnimators.OrderBy(animator => animator.transform.position.x).ToList();
        
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
        Debug.Log(index + " chosen");
        
        if (beforLastKey == index)
        {
            partsAnimators[beforLastKey].SwapSprites();
            beforLastKey = -1;
            howManyChois--;
            return;
        }
        
        if (lastKey == index)
        {
            partsAnimators[lastKey].SwapSprites();
            lastKey = -1;
            howManyChois--;
            return;
        }

        if (howManyChois >= 2)
        {
            Debug.Log("All parts are choosen");
        }
        else
        {
            howManyChois++;
            partsAnimators[index].SwapSprites();

            if (lastKey != -1)
            {
                beforLastKey = lastKey;
            }

            lastKey = index;
        }
    }

    private void SpawnButton(int placingInterval)
    {
        var spawnedButton = Instantiate(partPrefab, cellsSpawnParent.transform.position + new Vector3(placingInterval, 0, 0), Quaternion.identity);
        spawnedButton.transform.SetParent(cellsSpawnParent.transform);
        var buttonAnimation = spawnedButton.GetComponent<ButtonChooseAnimation>();
        partsAnimators.Add(buttonAnimation);
        parts.Add(spawnedButton);
    }

    public void WhenButtonClicked()
    {
        if (lastKey == -1 || beforLastKey == -1)
        {
            Debug.Log("No upgrades choosen");
            return;
        }

        upgradeButton.UpgradeChosenPart((UpgradeButton.Upgrades) lastKey, (UpgradeButton.Upgrades) beforLastKey);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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

    public void Initialize(int partsAmount, Sprite farmSprite, Sprite spiritBuildingSprite, Sprite militaryCampSprite, Sprite resourcesCenter)
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
        var sprites = new List<Sprite>()
        {
            farmSprite,
            spiritBuildingSprite,
            militaryCampSprite,
            resourcesCenter
        };
        
        for (int i = 0; i < parts.Count; i++)
        {
            int partIndex = i;
            parts[i].onClick.AddListener(delegate { ChoosePart((UpgradeButton.Upgrades)partIndex); });
            partsAnimators[i].SetIcon(sprites[i]);
        }
    }

    private void ChoosePart(UpgradeButton.Upgrades index)
    {
        if (beforLastKey == (int)index)
        {
            partsAnimators[beforLastKey].SwapSprites();
            beforLastKey = -1;
            howManyChois--;
            return;
        }
        
        if (lastKey == (int)index)
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
            partsAnimators[(int)index].SwapSprites();

            if (lastKey != -1)
            {
                beforLastKey = lastKey;
            }

            lastKey = (int)index;
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

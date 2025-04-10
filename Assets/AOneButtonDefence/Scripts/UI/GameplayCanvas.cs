using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField] private UIInfoButton partPrefab;
    [SerializeField] private UIInfoPanel infoPanel;
    [SerializeField] private GameObject cellsSpawnParent;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private int buttonsDistance = 100;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private StatisticViewInitializer statisticViewInitializer;

    private GameObject spawnedShopWindow;
    private GameObject spawnedSettingsWindow;
    private int partPlacingInterval = 0;
    private float startButtonsAmount;
    private List<UIInfoButton> parts = new List<UIInfoButton>();
    private List<ButtonChooseAnimation> partsAnimators = new List<ButtonChooseAnimation>();
    private int lastKey = -1;
    private int beforLastKey = -1;
    private int howManyChois = 0;
    private bool isShopWindowSetted = false;

    public UpgradeButton UpgradeButton => upgradeButton;

    public void Initialize(int partsAmount, BuildingsData buildingsData)
    {
        statisticViewInitializer.Initialize(ResourceData.ResourceType.Food, ResourceData.ResourceType.Warrior, ResourceData.ResourceType.Spirit, ResourceData.ResourceType.Material, ResourceData.ResourceType.Gem);
        
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
            BasicBuildingData currentBuildingData = buildingsData.Buildings[i];
            parts[i].Initialize(currentBuildingData, infoPanel);
            parts[i].Button.onClick.AddListener(delegate { ChoosePart(currentBuildingData.UpgradeType); });
            partsAnimators[i].SetIcon(currentBuildingData.Icon);
        }
        
        shopButton.onClick.RemoveAllListeners();
        shopButton.onClick.AddListener(ShowShopWindow);
        
        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(ShowSettingsWindow);
    }

    private void DetectShopWindow(GameObject window) => spawnedShopWindow = window;
    
    private void DetectSettingsWindow(GameObject window) => spawnedSettingsWindow = window;
    
    private void ShowShopWindow()
    {
        if (spawnedShopWindow != null)
            spawnedShopWindow.gameObject.SetActive(true);
    }
    
    private void ShowSettingsWindow()
    {
        if (spawnedSettingsWindow != null)
            spawnedSettingsWindow.gameObject.SetActive(true);
    }

    private void ChoosePart(BasicBuildingData.Upgrades index)
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

        upgradeButton.UpgradeChosenPart((BasicBuildingData.Upgrades) lastKey, (BasicBuildingData.Upgrades) beforLastKey);
    }

    private void OnEnable()
    {
        SkinPanel.ShopInitialized += DetectShopWindow;
        SoundSettings.SettingsInitialized += DetectSettingsWindow;
    }

    private void OnDisable()
    {
        SkinPanel.ShopInitialized -= DetectShopWindow;
        SoundSettings.SettingsInitialized -= DetectSettingsWindow;
    }
}

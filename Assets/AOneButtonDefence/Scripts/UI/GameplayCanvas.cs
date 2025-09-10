using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour
{
    [field : SerializeField] public GameObject ResourceInfo { get; private set; }
    [field : SerializeField] public GameObject UpgradeWindow { get; private set; }
    [field : SerializeField] public EnemiesCountIndicator EnemiesCountIndicator { get; private set; }
    [SerializeField] private UIInfoButton partPrefab;
    [SerializeField] private UIInfoPanel infoPanel;
    [SerializeField] private TextMeshProUGUI iconsText;
    [SerializeField] private GameObject cellsSpawnParent;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private int buttonsDistance = 100;
    [SerializeField] private Button shopOpenButton;
    [SerializeField] private Button settingsOpenButton;
    [SerializeField] private StatisticViewInitializer statisticViewInitializer;

    private UnityAction shopWindowHandler;
    private UnityAction settingsWindowHandler;
    private ClosableWindow spawnedShopWindow;
    private ClosableWindow spawnedSettingsWindow;
    private int partPlacingInterval = 0;
    private float startButtonsAmount;
    private List<UIInfoButton> parts = new List<UIInfoButton>();
    private List<ButtonChooseAnimation> partsAnimators = new List<ButtonChooseAnimation>();
    private int lastKey = -1;
    private int beforLastKey = -1;
    private int howManyChois = 0;
    private bool isShopWindowSetted = false;

    [field : SerializeField] public SoundSettings SoundSettings { get; private set; }

    public UpgradeButton UpgradeButton => upgradeButton;

    public void Initialize(int partsAmount, BuildingsData buildingsData)
    {
        statisticViewInitializer.Initialize(ResourceData.ResourceType.Food, ResourceData.ResourceType.Warrior, ResourceData.ResourceType.Spirit, ResourceData.ResourceType.Material, ResourceData.ResourceType.Gem);
        iconsText.text = "Выберите 2 здания для строительства.";
        
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
            SpawnButton(0);

            startButtonsAmount = (partsAmount - 1) / 2;
            partPlacingInterval = buttonsDistance;

            for (int i = 0; i < startButtonsAmount; i++)
            {
                SpawnButton(partPlacingInterval);
                SpawnButton(-partPlacingInterval);
                partPlacingInterval += buttonsDistance;
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
    }
    
    public void DetectSettingsWindow(ClosableWindow window)
    {
        spawnedSettingsWindow = window;
        settingsOpenButton.onClick.RemoveAllListeners();
        settingsWindowHandler = () => { SetSettingsWindowActive(true); };
        settingsOpenButton.onClick.AddListener(settingsWindowHandler);
        spawnedSettingsWindow.AddCloseListener(() => { SetShopWindowActive(false); });
    }

    private void DetectShopWindow(ClosableWindow window)
    {
        spawnedShopWindow = window;
        shopOpenButton.onClick.RemoveAllListeners();
        shopWindowHandler = () => { SetShopWindowActive(true); };
        shopOpenButton.onClick.AddListener(shopWindowHandler);
        spawnedShopWindow.AddCloseListener(() => { SetShopWindowActive(false); });
    }
    
    private void SetShopWindowActive(bool value)
    {
        if (spawnedShopWindow != null)
        {
            spawnedShopWindow.gameObject.SetActive(value);
            SetGameplayUIActive(!value);
        }
    }
    
    private void SetSettingsWindowActive(bool value)
    {
        if (spawnedSettingsWindow != null)
        {
            spawnedSettingsWindow.gameObject.SetActive(value);
            SetGameplayUIActive(!value);
        }
    }

    private void ChoosePart(BasicBuildingData.Upgrades index)
    {
        if (beforLastKey == (int)index)
        {
            partsAnimators[beforLastKey].SwapSprites();
            beforLastKey = -1;
            howManyChois--;
            UpdateUpgradeView();
            return;
        }
        
        if (lastKey == (int)index)
        {
            partsAnimators[lastKey].SwapSprites();
            lastKey = -1;
            howManyChois--;
            UpdateUpgradeView();
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
            UpdateUpgradeView();
        }
    }

    private void UpdateUpgradeView()
    {
        switch (howManyChois)
        {
            case 0:
                iconsText.text = "Выберите 2 здания для строительства.";
                upgradeButton.Deactivate();
                break;
            case 1:
                iconsText.text = "Выберите ещё одно здание!";
                upgradeButton.Deactivate();
                break;
            case 2:
                iconsText.text = "0ба здания выбраны. 0жидаем приказа!";
                upgradeButton.Activate();
                break;
        } 
    }

    private void SpawnButton(int placingInterval)
    {
        var spawnedButton = Instantiate(partPrefab, cellsSpawnParent.transform);
    
        var rect = spawnedButton.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(placingInterval, 0f);

        var buttonAnimation = spawnedButton.GetComponent<ButtonChooseAnimation>();
        partsAnimators.Add(buttonAnimation);
        parts.Add(spawnedButton);
    }

    private void SetGameplayUIActive(bool value)
    {
        UpgradeWindow.gameObject.SetActive(value);
        ResourceInfo.gameObject.SetActive(value);
    }

    private void DisableOpenableWindowButtons()
    {
        shopOpenButton.interactable = false;
        settingsOpenButton.interactable = false;
    }

    private void EnableOpenableWindowButton()
    {
        shopOpenButton.interactable = true;
        settingsOpenButton.interactable = true;
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
        UpgradeButton.UpgradesChoosen += DisableOpenableWindowButtons;
        UpgradeState.UpgradeStateStarted += EnableOpenableWindowButton;
        UpgradeState.UpgradeStateStarted += UpdateUpgradeView;
    }

    private void OnDisable()
    {
        SkinPanel.ShopInitialized -= DetectShopWindow;
        UpgradeButton.UpgradesChoosen -= DisableOpenableWindowButtons;
        UpgradeState.UpgradeStateStarted -= EnableOpenableWindowButton;
        UpgradeState.UpgradeStateStarted -= UpdateUpgradeView;
    }
}

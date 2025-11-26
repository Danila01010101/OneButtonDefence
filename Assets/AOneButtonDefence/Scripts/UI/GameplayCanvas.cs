using System;
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

    [field : SerializeField] public AudioSettings AudioSettings { get; private set; }

    public UpgradeButton UpgradeButton => upgradeButton;

    public void Initialize(int partsAmount, BuildingsData buildingsData)
    {
        statisticViewInitializer.Initialize(
            ResourceData.ResourceType.Food,
            ResourceData.ResourceType.Warrior,
            ResourceData.ResourceType.Spirit,
            ResourceData.ResourceType.Material,
            ResourceData.ResourceType.Gem);

        iconsText.text = "Выберите 2 здания для строительства.";

        var spawner = new BuildingButtonsSpawner(
            partPrefab,
            cellsSpawnParent.transform,
            buttonsDistance);

        var spawned = spawner.Spawn(partsAmount);

        parts = new List<UIInfoButton>(spawned);
        partsAnimators = new List<ButtonChooseAnimation>();

        for (int i = 0; i < parts.Count; i++)
        {
            var anim = parts[i].GetComponent<ButtonChooseAnimation>();
            partsAnimators.Add(anim);

            var data = buildingsData.Buildings[i];
            parts[i].Initialize(data, infoPanel);

            int captured = i;
            parts[i].Button.onClick.AddListener(() =>
            {
                ChoosePart(data.UpgradeType);
            });

            anim.SetIcon(data.Icon);
        }

        if (spawnedShopWindow != null)
            SetShopWindowActive(false);
    }
    
    public void DetectSettingsWindow(ClosableWindow window)
    {
        spawnedSettingsWindow = window;
        settingsOpenButton.onClick.RemoveAllListeners();
        settingsWindowHandler = () => { SetSettingsWindowActive(true); };
        settingsOpenButton.onClick.AddListener(settingsWindowHandler);
        spawnedSettingsWindow.AddCloseListener(() => { SetSettingsWindowActive(false); });
    }

    public void DetectShopWindow(ClosableWindow window)
    {
        spawnedShopWindow = window;
        shopOpenButton.onClick.RemoveAllListeners();
        shopWindowHandler = () => { SetShopWindowActive(true); };
        shopOpenButton.onClick.AddListener(shopWindowHandler);
        spawnedShopWindow.AddCloseListener(() => { SetShopWindowActive(false); });
    }
    
    private void SetShopWindowActive(bool value)
    {
        if (spawnedShopWindow == null) return;
        spawnedShopWindow.gameObject.SetActive(value);
        SetGameplayUIActive(!value);
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
                iconsText.text = "Оба здания выбраны. Ожидаем приказа!";
                upgradeButton.Activate();
                break;
        } 
    }
    
    private void SetGameplayUIActive(bool value)
    {
        if (UpgradeWindow != null) UpgradeWindow.SetActive(value);
        if (ResourceInfo != null) ResourceInfo.SetActive(value);
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
        UpgradeButton.UpgradesChoosen += DisableOpenableWindowButtons;
        UpgradeState.UpgradeStateStarted += EnableOpenableWindowButton;
        UpgradeState.UpgradeStateStarted += UpdateUpgradeView;
    }

    private void OnDisable()
    {
        UpgradeButton.UpgradesChoosen -= DisableOpenableWindowButtons;
        UpgradeState.UpgradeStateStarted -= EnableOpenableWindowButton;
        UpgradeState.UpgradeStateStarted -= UpdateUpgradeView;
    }
}

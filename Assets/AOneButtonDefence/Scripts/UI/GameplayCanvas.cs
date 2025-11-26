using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour
{
    [field: SerializeField] public GameObject ResourceInfo { get; private set; }
    [field: SerializeField] public GameObject UpgradeWindow { get; private set; }
    [field: SerializeField] public EnemiesCountIndicator EnemiesCountIndicator { get; private set; }

    [SerializeField] private UIInfoButton partPrefab;
    [SerializeField] private UIInfoPanel infoPanel;
    [SerializeField] private TextMeshProUGUI iconsText;
    [SerializeField] private GameObject cellsSpawnParent;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private int buttonsDistance = 100;
    [SerializeField] private Button shopOpenButton;
    [SerializeField] private Button settingsOpenButton;
    [SerializeField] private StatisticViewInitializer statisticViewInitializer;
    [SerializeField] private int randomBuildingsCount = 2;

    private UnityAction shopWindowHandler;
    private UnityAction settingsWindowHandler;
    private ClosableWindow spawnedShopWindow;
    private ClosableWindow spawnedSettingsWindow;
    private RandomBuildingsSelector buildingsSelector;

    private List<UIInfoButton> parts = new List<UIInfoButton>();
    private List<ButtonChooseAnimation> partsAnimators = new List<ButtonChooseAnimation>();
    private List<BasicBuildingData> currentSelection = new List<BasicBuildingData>();

    private int lastKey = -1;
    private int beforLastKey = -1;
    private int howManyChois = 0;

    [field: SerializeField] public AudioSettings AudioSettings { get; private set; }

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
        buildingsSelector = new RandomBuildingsSelector(buildingsData.Buildings);

        SpawnNewBuildingsSet();
    }

    private void ChoosePart(int index)
    {
        if (index < 0 || index >= partsAnimators.Count) return;

        if (beforLastKey == index)
        {
            partsAnimators[beforLastKey].SwapSprites();
            beforLastKey = -1;
            howManyChois--;
            UpdateUpgradeView();
            return;
        }

        if (lastKey == index)
        {
            partsAnimators[lastKey].SwapSprites();
            lastKey = -1;
            howManyChois--;
            UpdateUpgradeView();
            return;
        }

        if (howManyChois >= 2)
        {
            return;
        }

        howManyChois++;
        partsAnimators[index].SwapSprites();

        if (lastKey != -1)
            beforLastKey = lastKey;

        lastKey = index;

        UpdateUpgradeView();
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

    private void SpawnNewBuildingsSet()
    {
        ClearPreviousButtons();

        currentSelection = buildingsSelector.GetSelection(randomBuildingsCount);

        var spawner = new BuildingButtonsSpawner(
            partPrefab,
            cellsSpawnParent.transform,
            buttonsDistance);

        var spawned = spawner.Spawn(currentSelection.Count);

        parts = new List<UIInfoButton>(spawned);
        partsAnimators = new List<ButtonChooseAnimation>();

        for (int i = 0; i < parts.Count; i++)
        {
            var data = currentSelection[i];
            var anim = parts[i].GetComponent<ButtonChooseAnimation>();

            partsAnimators.Add(anim);
            parts[i].Initialize(data, infoPanel);

            int capturedIndex = i;
            parts[i].Button.onClick.AddListener(() => ChoosePart(capturedIndex));

            anim.SetIcon(data.Icon);
        }

        UpdateUpgradeView();
    }

    private void ClearPreviousButtons()
    {
        foreach (Transform child in cellsSpawnParent.transform)
            Destroy(child.gameObject);

        parts.Clear();
        partsAnimators.Clear();
        currentSelection.Clear();

        lastKey = -1;
        beforLastKey = -1;
        howManyChois = 0;
    }

    public void WhenButtonClicked()
    {
        if (lastKey == -1 || beforLastKey == -1)
            return;

        var first = currentSelection[lastKey].UpgradeType;
        var second = currentSelection[beforLastKey].UpgradeType;

        upgradeButton.UpgradeChosenPart(first, second);
    }
    
    public void DetectSettingsWindow(ClosableWindow window)
    {
        spawnedSettingsWindow = window;

        settingsOpenButton.onClick.RemoveAllListeners();
        settingsOpenButton.onClick.AddListener(() => SetSettingsWindowActive(true));

        if (spawnedSettingsWindow != null)
            spawnedSettingsWindow.AddCloseListener(() => SetSettingsWindowActive(false));
    }
    
    private void SetSettingsWindowActive(bool value)
    {
        if (spawnedSettingsWindow == null) return;

        spawnedSettingsWindow.gameObject.SetActive(value);
        SetGameplayUIActive(!value);
    }

    private void SetShopWindowActive(bool value)
    {
        if (spawnedShopWindow == null) return;

        spawnedShopWindow.gameObject.SetActive(value);
        SetGameplayUIActive(!value);
    }

    private void OnEnable()
    {
        UpgradeButton.UpgradesChoosen += DisableOpenableWindowButtons;
        UpgradeState.UpgradeStateStarted += EnableOpenableWindowButton;
        UpgradeState.UpgradeStateStarted += UpdateUpgradeView;
        UpgradeState.UpgradeStateStarted += SpawnNewBuildingsSet;
    }

    private void OnDisable()
    {
        UpgradeButton.UpgradesChoosen -= DisableOpenableWindowButtons;
        UpgradeState.UpgradeStateStarted -= EnableOpenableWindowButton;
        UpgradeState.UpgradeStateStarted -= UpdateUpgradeView;
        UpgradeState.UpgradeStateStarted -= SpawnNewBuildingsSet;
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
}
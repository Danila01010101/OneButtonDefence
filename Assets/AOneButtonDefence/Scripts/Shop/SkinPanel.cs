using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ClosableWindow))]
[RequireComponent(typeof(AudioSource))]
public class SkinPanel : MonoBehaviour
{
    [SerializeField] private ShopSkinShower exampleSkinShowerPrefab;
    [SerializeField] private Vector3 exampleSkinChangerPosition;
    [SerializeField] private Vector3 exampleSkinChangerEulerAngles;
    [SerializeField] private AudioSource audioSource;
    [Header("Скины")]
    public List<SkinData> SkinList = new List<SkinData>();
    public int CurrentChose;
    public int ChosenSkin = 0;
    [Header("UI")]
    public TMP_Text SkinName;
    public TMP_Text SkinLore;
    public TMP_Text SkinCost;
    public TMP_Text BuyButtonText;
    public TMP_Text RewardButtonText;
    public Button RewardButton;
    public Image CurrentSkinSprite;
    public Image NextSkinSprite;
    public Image PrevSkinSprite;
    public Image SelectSkinSprite;

    private AudioSource buyButtonAudio;
    private SkinOpenSoundPlayer skinOpenSoundPlayer;

    public static Action<SkinData> SkinChanged;
    public static Action<ClosableWindow> ShopInitialized;
    public static Action<SkinData> SkinBought;
    public static Action ShopEnabled;
    public static Action ShopDisabled;

    public static ClosableWindow CurrentShopWindow { get; private set; }
    public AudioSource SkinsAudioSource => audioSource;

    private bool firstRewardTaken;
    private ShopSkinShower spawnedShopSkinShower;
    private SkinsSave saveData;
    private const string SaveFileName = "skins_save.json";

    public void Initialize(IInput input)
    {
        if (exampleSkinShowerPrefab != null && UIGameObjectShower.Instance != null)
        {
            spawnedShopSkinShower = UIGameObjectShower.Instance.RenderPrefab(exampleSkinShowerPrefab, exampleSkinChangerPosition, Quaternion.Euler(exampleSkinChangerEulerAngles));
            if (spawnedShopSkinShower != null)
                spawnedShopSkinShower.Initialize(input);
        }

        buyButtonAudio = GetComponent<AudioSource>();
        LoadOrCreateSave();
        CurrentShopWindow = GetComponent<ClosableWindow>();
        ShopInitialized?.Invoke(CurrentShopWindow);

        SafeInitializeUI();
    }

    private void Start()
    {
        ClearSavedSkins();
        
        if (buyButtonAudio == null && buyButtonAudio != null)
            buyButtonAudio = GetComponent<AudioSource>();
        if (skinOpenSoundPlayer == null && buyButtonAudio != null)
            skinOpenSoundPlayer = new SkinOpenSoundPlayer(buyButtonAudio);

        if (saveData == null)
            LoadOrCreateSave();

        SafeInitializeUI();
    }

    private void SafeInitializeUI()
    {
        if (SkinList == null || SkinList.Count == 0) return;
        CurrentChose = MaxMinIndex(CurrentChose, SkinList);
        ChangeCurrentChose(0);
        SelectSkin(ChosenSkin);
    }

    public void ChangeCurrentChose(int num)
    {
        if (SkinList == null || SkinList.Count == 0) return;

        CurrentChose += num;
        int nextChose;
        int prevChose;

        CurrentChose = MaxMinIndex(CurrentChose, SkinList);
        nextChose = CurrentChose + 1;
        prevChose = CurrentChose - 1;
        nextChose = MaxMinIndex(nextChose, SkinList);
        prevChose = MaxMinIndex(prevChose, SkinList);

        var currentSkin = SkinList[CurrentChose];
        var nextSkin = SkinList[nextChose];
        var prevSkin = SkinList[prevChose];

        if (CurrentSkinSprite != null) CurrentSkinSprite.sprite = currentSkin.Icon;
        if (NextSkinSprite != null) NextSkinSprite.sprite = nextSkin.Icon;
        if (PrevSkinSprite != null) PrevSkinSprite.sprite = prevSkin.Icon;

        if (SkinName != null) SkinName.text = currentSkin.SkinName ?? "";
        if (SkinLore != null) SkinLore.text = currentSkin.SkinLore ?? "";
        if (SkinCost != null) SkinCost.text = currentSkin.Cost.ToString();

        if (spawnedShopSkinShower != null && spawnedShopSkinShower.GnomeSkinChanger != null)
        {
            string id = GetSkinId(currentSkin);
            bool unlocked = saveData != null && saveData.IsUnlocked(id);
            spawnedShopSkinShower?.GnomeSkinChanger?.ChangeSkin(currentSkin, unlocked);
        }

        ChangeText();
    }

    public void SetSkin()
    {
        if (SkinList == null || SkinList.Count == 0) return;

        var skin = SkinList[CurrentChose];
        string id = GetSkinId(skin);

        if (saveData.IsUnlocked(id))
        {
            SelectSkin(CurrentChose);
            return;
        }

        int gems = GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.Gem);
        if (skin.Cost > gems)
        {
            Debug.Log("Недостаточно алмазов");
            return;
        }

        saveData.Unlock(id);
        SaveToDisk();
        var currentSkin = SkinList[CurrentChose];
        spawnedShopSkinShower?.GnomeSkinChanger?.ChangeSkin(currentSkin, true);
        SkinBought?.Invoke(skin);
        SelectSkin(CurrentChose);
    }

    private void SelectSkin(int index)
    {
        if (SkinList == null || SkinList.Count == 0) return;
        index = MaxMinIndex(index, SkinList);
        ChosenSkin = index;
        var skin = SkinList[index];
        SkinChanged?.Invoke(skin);
        ChangeText();
        if (SelectSkinSprite != null) SelectSkinSprite.sprite = skin.Icon;
    }

    public int MaxMinIndex<T>(int index, List<T> list)
    {
        if (list == null || list.Count == 0) return 0;
        if (index >= list.Count) index = 0;
        if (index < 0) index = list.Count - 1;
        return index;
    }

    private void EnablePanel()
    {
        if (spawnedShopSkinShower != null)
            spawnedShopSkinShower.ShowExampleSkin();

        ShopEnabled?.Invoke();
    }

    private void DisablePanel()
    {
        if (spawnedShopSkinShower != null)
            spawnedShopSkinShower.HideExampleSkin();

        ShopDisabled?.Invoke();
    }

    private void ChangeText()
    {
        if (SkinList == null || SkinList.Count == 0 || saveData == null) return;

        var skin = SkinList[CurrentChose];
        string id = GetSkinId(skin);

        bool unlocked = saveData.IsUnlocked(id);

        if (unlocked)
        {
            if (RewardButtonText != null)
            {
                if (firstRewardTaken == false && CurrentChose == ChosenSkin)
                {
                    RewardButtonText.text = "Этот облик уже доступен";
                }
                else
                {
                    RewardButtonText.text = "Получить 25 кристаллов за рекламу";
                }
            }
                
            if (BuyButtonText != null)
                BuyButtonText.text = (CurrentChose == ChosenSkin) ? "Выбрано" : "Применить";
            if (SkinCost != null) SkinCost.text = "";
        }
        else
        {
            int gems = GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.Gem);
            RewardButtonText.text = firstRewardTaken ? "Получить 25 кристаллов за рекламу" : "Получить без кристаллов!\n (Просмотр рекламы)";
            
            if (SkinCost != null) SkinCost.text = gems > skin.Cost ?  "" : "Не хватает " + (skin.Cost - gems) + " алмазов";
            if (BuyButtonText != null) BuyButtonText.text = "купить";
        }
    }
    
    private void DetectFirstRewardTaken() => firstRewardTaken = true;

    private void OnEnable()
    {
        RewardGemsActivator.FirstRewardActivated += DetectFirstRewardTaken;
        EnablePanel();
    }

    private void OnDisable()
    {
        RewardGemsActivator.FirstRewardActivated -= DetectFirstRewardTaken;
        DisablePanel();
    }

    private void OnDestroy()
    {
        skinOpenSoundPlayer?.Dispose();
        if (CurrentShopWindow == GetComponent<ClosableWindow>()) CurrentShopWindow = null;
    }

    #region Save system (local, simple JSON)

    [Serializable]
    private class SkinSaveEntry
    {
        public string id;
        public bool unlocked;
    }

    [Serializable]
    private class SkinsSave
    {
        public List<SkinSaveEntry> entries = new List<SkinSaveEntry>();

        public bool IsUnlocked(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            var e = entries.Find(x => x.id == id);
            return e != null && e.unlocked;
        }

        public void Unlock(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            var e = entries.Find(x => x.id == id);
            if (e == null)
            {
                entries.Add(new SkinSaveEntry { id = id, unlocked = true });
            }
            else
            {
                e.unlocked = true;
            }
        }

        public void Ensure(string id, bool unlockedByDefault)
        {
            if (string.IsNullOrEmpty(id)) return;
            var e = entries.Find(x => x.id == id);
            if (e == null)
            {
                entries.Add(new SkinSaveEntry { id = id, unlocked = unlockedByDefault });
            }
        }
    }

    private void LoadOrCreateSave()
    {
        try
        {
            var path = Path.Combine(Application.persistentDataPath, SaveFileName);
            if (!File.Exists(path))
            {
                saveData = new SkinsSave();
                SeedSaveFromSO();
                SaveToDisk();
                return;
            }

            var json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SkinsSave>(json) ?? new SkinsSave();
            SeedSaveFromSO();
            SaveToDisk();
        }
        catch (Exception)
        {
            saveData = new SkinsSave();
            SeedSaveFromSO();
            try { SaveToDisk(); } catch { }
        }
    }

    private void SeedSaveFromSO()
    {
        if (SkinList == null) return;
        foreach (var skin in SkinList)
        {
            if (skin == null) continue;
            string id = GetSkinId(skin);
            bool defaultUnlocked = skin.Unlocked;
            saveData.Ensure(id, defaultUnlocked);
        }
    }

    private void SaveToDisk()
    {
        try
        {
            var path = Path.Combine(Application.persistentDataPath, SaveFileName);
            var json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, json);
        }
        catch (Exception) { }
    }

    private string GetSkinId(SkinData skin)
    {
        if (skin == null) return null;
        return skin.name ?? skin.SkinName ?? Guid.NewGuid().ToString();
    }
    
    private void ClearSavedSkins()
    {
        PlayerPrefs.DeleteAll();
        
        try
        {
            var path = Path.Combine(Application.persistentDataPath, SaveFileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Скины: сохранение очищено при старте билда.");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Не удалось очистить сохранение скинов: " + e.Message);
        }
    }

    #endregion
}
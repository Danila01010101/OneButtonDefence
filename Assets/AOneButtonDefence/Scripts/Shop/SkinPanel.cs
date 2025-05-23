using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanel : MonoBehaviour
{
    [SerializeField] private ShopSkinShower exampleSkinShowerPrefab;
    [SerializeField] private Vector3 exampleSkinChangerPosition;
    [SerializeField] private Vector3 exampleSkinChangerEulerAngles;
    [Header("???? ??????")]
    public List<SkinData> SkinList;
    public int CurrentChose;
    public int ChosenSkin = 0;
    [Header("UI")]
    public TMP_Text SkinName;
    public TMP_Text SkinLore;
    public TMP_Text SkinCost;
    public TMP_Text BuyButtonText;
    public Image CurrentSkinSprite;
    public Image NextSkinSprite;
    public Image PrevSkinSprite;
    public Image SelectSkinSprite;
    public Image BuySkin;

    public static Action<Mesh, Material> SkinChanged;
    public static Action<GameObject> ShopInitialized;
    public static Action<int> SkinBought;
    public static Action ShopEnabled;
    public static Action ShopDisabled;

    private ShopSkinShower spawnedShopSkinShower;
    
    public void Initialize(IInput input)
    {
        spawnedShopSkinShower = UIGameObjectShower.Instance.RenderPrefab(exampleSkinShowerPrefab, exampleSkinChangerPosition, Quaternion.Euler(exampleSkinChangerEulerAngles));
        spawnedShopSkinShower.Initialize(input);
        ChangeCurrentChose(0);
        SelectSkin(0);
        ShopInitialized?.Invoke(gameObject);
        gameObject.SetActive(false);
    }

    public void ChangeCurrentChose(int num)
    {
        CurrentChose += num;
        int nextChose;
        int prevChose;

        CurrentChose = MaxMinIndex(CurrentChose, SkinList);
        nextChose = CurrentChose + 1;
        prevChose = CurrentChose - 1;
        nextChose = MaxMinIndex(nextChose, SkinList);
        prevChose = MaxMinIndex(prevChose, SkinList);

        CurrentSkinSprite.sprite = SkinList[CurrentChose].Icon;
        NextSkinSprite.sprite = SkinList[nextChose].Icon;
        PrevSkinSprite.sprite = SkinList[prevChose].Icon;

        SkinName.text = SkinList[CurrentChose].SkinName;
        SkinLore.text = SkinList[CurrentChose].SkinLore;
        SkinCost.text = SkinList[CurrentChose].Cost.ToString();
        
        spawnedShopSkinShower.GnomeSkinChanger.ChangeSkin(SkinList[CurrentChose].Mesh, SkinList[CurrentChose].Material);

        ChangeText();
    }

    public void SetSkin()
    {
        if (SkinList[CurrentChose].Unlocked == true)
        {
            SelectSkin(CurrentChose);
        }
        else
        {
            if (SkinList[CurrentChose].Cost > GameResourcesCounter.GetResourceAmount(ResourceData.ResourceType.Gem))
            {
                Debug.Log("Недостаточно алмазов");
                return;
            }

            SkinBought?.Invoke(SkinList[CurrentChose].Cost);
            SkinList[CurrentChose].Unlocked = true;
            SelectSkin(CurrentChose);
        }
    }

    private void SelectSkin(int index)
    {
        ChosenSkin = index;
        SkinChanged?.Invoke(SkinList[index].Mesh, SkinList[index].Material);
        ChangeText();
        SelectSkinSprite.sprite = SkinList[index].Icon;
    }
    
    public int MaxMinIndex<T>(int index, List<T> list)
    {
        if (index == list.Count)
        {
            index = 0;
        }
        
        if (index == -1)
        {
            index = list.Count - 1;
        }
        
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
        if (SkinList[CurrentChose].Unlocked)
        {
            BuyButtonText.text = "Применить";
            if (CurrentChose == ChosenSkin)
            {
                BuyButtonText.text = "Выбрано";
            }
            SkinCost.text = "";
        }
        else
        {
            SkinCost.text = "стоит " + SkinList[CurrentChose].Cost + " алмазов";
            BuyButtonText.text = "купить";
        }
    }
    private void OnEnable() => EnablePanel();

    private void OnDisable() => DisablePanel();
}

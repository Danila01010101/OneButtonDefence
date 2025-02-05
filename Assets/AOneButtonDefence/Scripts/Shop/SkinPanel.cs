using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanel : MonoBehaviour
{
    [Header("?????? ??? ???????? ???????? ????")]
    public GameObject ChangeGameObject;
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

    [HideInInspector]
    public static Action<Mesh, Material> SkinChanged;

    private MeshFilter _meshFilter;
    private Renderer _renderer;

    private void Start()
    {
        _meshFilter = ChangeGameObject.GetComponent<MeshFilter>();
        _renderer = ChangeGameObject.GetComponent<Renderer>();
        ////????????!
        ResourcesCounter.Instance.Data.GemsAmount += 1000;

        ChangeCurrentChose(0);
        SelectSkin(0);
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

        if (SkinList[CurrentChose].Unlocked)
        {
            BuyButtonText.text = "??????? ????";
            SkinCost.text = "???????";
        }
        else
        {
            BuyButtonText.text = "??????";
        }
        if (CurrentChose == ChosenSkin)
        {
            BuyButtonText.text = "??????";
        }

        _meshFilter.mesh = SkinList[CurrentChose].Mesh;
        _renderer.material = SkinList[CurrentChose].Material;
    }

    public void SetSkin()
    {
        if (SkinList[CurrentChose].Unlocked == true)
        {
            SelectSkin(CurrentChose);
        }
        else
        {
            if (SkinList[CurrentChose].Cost > ResourcesCounter.Instance.Data.GemsAmount)
            {
                Debug.Log("??? ?????, ????");
                return;
            }
            ResourcesCounter.Instance.Data.GemsAmount -= SkinList[CurrentChose].Cost;

            SkinList[CurrentChose].Unlocked = true;
            SelectSkin(CurrentChose);
        }
    }

    private void SelectSkin(int index)
    {
        SkinChanged?.Invoke(SkinList[index].Mesh, SkinList[index].Material);
        BuyButtonText.text = "??????";
        SkinCost.text = "???????";
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanel : MonoBehaviour
{
    [Header("Объект для которого меняется скин")]
    public GameObject ChangeGameObject;
    [Header("Лист скинов")]
    public List<SkinData> SkinList;
    public int CurrentChose;
    [Header("UI")]
    public TMP_Text SkinName;
    public TMP_Text SkinLore;
    public TMP_Text SkinCost;
    public TMP_Text BuyButtonText;

    public Image CurrentSkinSprite;
    public Image NextSkinSprite;
    public Image PrevSkinSprite;

    public Image BuySkin;

    private MeshFilter _meshFilter;
    private Renderer _renderer;

    private void Start()
    {
        _meshFilter = ChangeGameObject.GetComponent<MeshFilter>();
        _renderer = ChangeGameObject.GetComponent<Renderer>();

        ChangeCurrentChose(0);
    }

    public void ChangeCurrentChose(int num)
    {
        CurrentChose += num;
        int nextChose = 0;
        int prevChose = 0;

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
            BuyButtonText.text = "";
        }
        else 
        {
            BuyButtonText.text = "Купить";
        }

        _meshFilter.mesh = SkinList[CurrentChose].Mesh;
        _renderer.material = SkinList[CurrentChose].Material;
    }

    //public void SetSkin()
    //{
    //    if (SkinList[CurrentChose].Unlocked == false)
    //    {
    //        if 
    //    }
    //}


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

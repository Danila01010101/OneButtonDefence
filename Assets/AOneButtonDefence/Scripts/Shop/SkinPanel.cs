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
    public Image CurrentSkinSprite;
    public Image NextSkinSprite;
    public Image PrevSkinSprite;
    public Image BuySkin;

    private Mesh _mesh;
    private Material _material;

    private void Start()
    {
        _mesh = ChangeGameObject.GetComponent<MeshFilter>().mesh;
        _material = ChangeGameObject.GetComponent<Renderer>().material;

        ChangeCurrentChose(0);
    }

    private void Update()
    {
        ChangeCurrentChose(0);
    }



    public void ChangeCurrentChose(int num)
    {
        CurrentChose += num;

        CurrentSkinSprite.sprite = SkinList[CurrentChose].Icon;

        if (CurrentChose + 1 < SkinList.Count) 
        {
            NextSkinSprite.sprite = SkinList[CurrentChose + 1].Icon;
        }
        else
        {
            NextSkinSprite.sprite = SkinList[0].Icon;
        }

        //if (CurrentChose - 1 > 0)
        //{
        //    PrevSkinSprite.sprite = SkinList[SkinList.Count - 1].Icon;
        //}
        //else 
        //{
        //    NextSkinSprite.sprite = SkinList[CurrentChose - 1].Icon;
        //}

        SkinName.text = SkinList[CurrentChose].SkinName;
        SkinLore.text = SkinList[CurrentChose].SkinLore;
        _mesh = SkinList[CurrentChose].Mesh;
        _material = SkinList[CurrentChose].Material;
    }
}

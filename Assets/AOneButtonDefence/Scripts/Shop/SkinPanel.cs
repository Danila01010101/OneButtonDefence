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

        if (CurrentChose < 0) 
        {
            CurrentChose = SkinList.Count - 1;
        }
        if (CurrentChose == SkinList.Count)
        {
            CurrentChose = 0;
        }

        CurrentSkinSprite.sprite = SkinList[CurrentChose].Icon;

        if (CurrentChose + 1 < SkinList.Count) 
        {
            NextSkinSprite.sprite = SkinList[CurrentChose + 1].Icon;
        }
        else
        {
            NextSkinSprite.sprite = SkinList[0].Icon;
        }

        if (CurrentChose == 0)
        {
            PrevSkinSprite.sprite = SkinList[SkinList.Count - 1].Icon;
        }
        else
        {
            NextSkinSprite.sprite = SkinList[CurrentChose - 1].Icon;
        }

        SkinName.text = SkinList[CurrentChose].SkinName;
        SkinLore.text = SkinList[CurrentChose].SkinLore;
        _meshFilter.mesh = SkinList[CurrentChose].Mesh;
        _renderer.material = SkinList[CurrentChose].Material;
    }
}

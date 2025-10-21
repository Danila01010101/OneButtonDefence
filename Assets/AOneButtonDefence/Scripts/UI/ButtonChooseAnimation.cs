using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChooseAnimation : MonoBehaviour
{
    [SerializeField] private List<Image> icons;
    [SerializeField] private Image onImage;
    [SerializeField] private Image offImage;

    public void SetIcon(Sprite sprite)
    {
        foreach (var icon in icons)
        {
            icon.sprite = sprite;
        }
    }
    
    public void SwapSprites()
    {
        if (onImage.gameObject.activeSelf == false)
        {
            onImage.gameObject.SetActive(true);
            offImage.gameObject.SetActive(false);
        }
        else
        {
            onImage.gameObject.SetActive(false);
            offImage.gameObject.SetActive(true);
        }
    }
}
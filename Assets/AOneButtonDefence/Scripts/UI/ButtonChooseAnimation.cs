using UnityEngine;
using UnityEngine.UI;

public class ButtonChooseAnimation : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image onImage;
    [SerializeField] private Image offImage;

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
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
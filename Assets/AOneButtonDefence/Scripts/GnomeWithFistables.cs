using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GnomeWithFistables : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private RectTransform endRect;
    private float duration;
    private Image thisImage;
    
    private void Animation()
    {
        thisImage = GetComponent<Image>();
        float distance = Mathf.Abs(endRect.anchoredPosition.x - thisImage.rectTransform.anchoredPosition.x);
        duration = distance / speed;
        thisImage.rectTransform.DOAnchorPosX(endRect.anchoredPosition.x, duration).SetEase(Ease.Linear);
    }

    public void Initialize(RectTransform endRectTransform)
    {
        endRect = endRectTransform;
        gameObject.AddComponent<Button>().onClick.AddListener(OnClick);
        Animation();
    }

    private void OnClick()
    {
        Destroy(gameObject);
        //GiveDiamonds
    }
}

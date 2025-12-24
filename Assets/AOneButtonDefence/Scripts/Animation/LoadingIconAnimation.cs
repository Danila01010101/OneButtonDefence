using UnityEngine;
using UnityEngine.UI;

public class LoadingIconAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float blinkSpeed = 1f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 1f;
    [SerializeField] private AnimationCurve blinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("References")]
    [SerializeField] private Image targetImage;
    
    private CanvasGroup canvasGroup;
    private float currentTime;
    private bool isAnimating = true;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
            
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        isAnimating = true;
        currentTime = 0f;
        
        if (canvasGroup != null)
            canvasGroup.alpha = maxAlpha;
    }

    private void OnDisable()
    {
        isAnimating = false;
        
        if (canvasGroup != null)
            canvasGroup.alpha = maxAlpha;
    }

    private void Update()
    {
        if (!isAnimating) return;
        
        currentTime += Time.deltaTime * blinkSpeed;
        
        float t = Mathf.PingPong(currentTime, 1f);
        float curveValue = blinkCurve.Evaluate(t);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, curveValue);
        
        if (canvasGroup != null)
            canvasGroup.alpha = alpha;
        else if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = alpha;
            targetImage.color = color;
        }
    }

    public void StartAnimation()
    {
        isAnimating = true;
        currentTime = 0f;
        enabled = true;
    }

    public void StopAnimation()
    {
        isAnimating = false;
        
        if (canvasGroup != null)
            canvasGroup.alpha = maxAlpha;
        else if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = maxAlpha;
            targetImage.color = color;
        }
        
        enabled = false;
    }

    public void SetAnimationSpeed(float speed)
    {
        blinkSpeed = Mathf.Max(0.1f, speed);
    }

    public void SetAlphaRange(float min, float max)
    {
        minAlpha = Mathf.Clamp01(min);
        maxAlpha = Mathf.Clamp01(max);
    }
}
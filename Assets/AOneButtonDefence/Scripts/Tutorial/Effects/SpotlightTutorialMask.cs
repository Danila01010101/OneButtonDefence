using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(RawImage))]
public class SpotlightTutorialMask : MonoBehaviour
{
    [SerializeField] private float featherSize = 80f;
    [SerializeField] private float animationDuration = 0.3f;

    private Material materialInstance;
    private RectTransform canvasRectTransform;
    private Tweener moveTween;
    private Tweener moveTweenMax;
    private static SpotlightTutorialMask instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        RawImage image = GetComponent<RawImage>();
        materialInstance = Instantiate(image.material);
        image.material = materialInstance;

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogWarning("Этот скрипт работает только с ScreenSpaceOverlay Canvas.");
        }

        canvasRectTransform = canvas.GetComponent<RectTransform>();

        materialInstance.SetVector("_ScreenResolution", new Vector4(Screen.width, Screen.height, 0, 0));

        materialInstance.SetVector("_RectMin", new Vector4(Screen.width / 2, Screen.height / 2, 0, 0));
        materialInstance.SetVector("_RectMax", new Vector4(Screen.width / 2, Screen.height / 2, 0, 0));

        DisableMask();
    }

    private void ChangeMaskTarget(RectTransform target)
    {
        SetActive(true);

        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        Vector2 minScreenPos = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 maxScreenPos = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        moveTween?.Kill();
        moveTweenMax?.Kill();

        Vector4 currentMin = materialInstance.GetVector("_RectMin");
        Vector4 currentMax = materialInstance.GetVector("_RectMax");

        moveTween = DOTween.To(() => currentMin, v =>
        {
            materialInstance.SetVector("_RectMin", v);
        }, new Vector4(minScreenPos.x, minScreenPos.y, 0, 0), animationDuration)
        .SetEase(Ease.InOutQuad)
        .SetUpdate(true);

        moveTweenMax = DOTween.To(() => currentMax, v =>
        {
            materialInstance.SetVector("_RectMax", v);
        }, new Vector4(maxScreenPos.x, maxScreenPos.y, 0, 0), animationDuration)
        .SetEase(Ease.InOutQuad)
        .SetUpdate(true);

        materialInstance.SetVector("_Feather", new Vector4(featherSize, featherSize, 0, 0));
    }

    private void SetActive(bool state) => gameObject.SetActive(state);

    public static void SetNewTarget(RectTransform target) => instance.ChangeMaskTarget(target);

    public static void DisableMask() => instance.SetActive(false);
}
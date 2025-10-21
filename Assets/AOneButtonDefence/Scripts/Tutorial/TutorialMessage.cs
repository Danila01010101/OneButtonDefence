using System;
using TMPro;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float edgePadding = 40f;
    [SerializeField] private float spacing = 125f;

    public float EngePadding => edgePadding;
    public float Spacing => spacing;
    
    public event Action OnClosed;
    public static event Action OnSkipButtonPressed;
    private GameObject target;
    
    public static event Action TutorialWindowDestroyed;

    // Теперь только заполняем контент и сохраняем target — поинтер не инициализируется здесь
    public void Setup(GameObject target, string message, Action onClosed = null)
    {
        this.target = target;
        messageText.text = message;
        OnClosed += onClosed;
    }

    // Вызывать ПОСЛЕ того, как окно уже окончательно позиционировано (TutorialManager делает это)
    public void InitializePointer()
    {
        if (target == null || pointer == null)
            return;

        if (target.GetComponent<RectTransform>()) SetupForUI();
        else SetupForWorldObject();
    }

    private void SetupForUI()
    {
        var targetRectTransform = target.GetComponent<RectTransform>();
        if (targetRectTransform == null)
            return;

        var worldPointer = pointer.GetComponent<WorldPointer>();
        if (worldPointer != null)
            Destroy(worldPointer);

        var uiPointer = pointer.GetComponent<UIPointer>();
        if (uiPointer == null)
            uiPointer = pointer.gameObject.AddComponent<UIPointer>();

        uiPointer.Initialize(targetRectTransform, pointer);
    }

    private void SetupForWorldObject()
    {
        var uiPointer = pointer.GetComponent<UIPointer>();
        if (uiPointer != null)
            Destroy(uiPointer);

        var worldPointer = pointer.GetComponent<WorldPointer>();
        if (worldPointer == null)
            worldPointer = pointer.gameObject.AddComponent<WorldPointer>();

        worldPointer.Initialize(target.transform, pointer);
    }

    public void SkipTutorial()
    {
        OnSkipButtonPressed?.Invoke();
        Close();
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SpotlightTutorialMask.DisableMask();
        OnClosed?.Invoke();
        OnClosed = null;
        TutorialWindowDestroyed?.Invoke();
        TimeManager.SetTimeScale(1f);
    }
}
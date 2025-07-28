using System;
using TMPro;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private RectTransform pointer;

    public event Action OnClosed;
    private GameObject target;

    public void Setup(GameObject target, string message)
    {
        this.target = target;
        messageText.text = message;

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

    public void Close()
    {
        OnClosed?.Invoke();
        Destroy(gameObject);
    }
}
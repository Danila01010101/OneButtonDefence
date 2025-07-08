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
        var rt = target.GetComponent<RectTransform>();
        pointer.gameObject.AddComponent<UIPointer>().Initialize(rt, pointer);
    }

    private void SetupForWorldObject()
    {
        pointer.gameObject.AddComponent<WorldPointer>().Initialize(target.transform, pointer);
    }

    public void Close()
    {
        OnClosed?.Invoke();
        Destroy(gameObject);
    }
}
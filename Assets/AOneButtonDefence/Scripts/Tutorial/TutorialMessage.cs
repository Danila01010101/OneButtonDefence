using System;
using TMPro;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private RectTransform _pointer;

    public event Action OnClosed;
    private GameObject _target;

    public void Setup(GameObject target, string message)
    {
        _target = target;
        _messageText.text = message;

        // Автоматическое определение типа объекта
        if (target.GetComponent<RectTransform>()) SetupForUI();
        else SetupForWorldObject();
    }

    private void SetupForUI()
    {
        var rt = _target.GetComponent<RectTransform>();
        _pointer.gameObject.AddComponent<UIPointer>().Initialize(rt, _pointer);
    }

    private void SetupForWorldObject()
    {
        _pointer.gameObject.AddComponent<WorldPointer>().Initialize(_target.transform, _pointer);
    }

    public void Close()
    {
        OnClosed?.Invoke();
        Destroy(gameObject);
    }
}
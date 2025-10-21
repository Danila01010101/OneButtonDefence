using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClosableWindow : MonoBehaviour
{
	[SerializeField] private Button closeButton;

	private UnityAction handler;

	private void Start()
	{
		AddCloseListener(CloseWindow);
	}

	public void AddCloseListener(Action onClose)
	{
		handler = () => onClose?.Invoke();
		closeButton.onClick.AddListener(handler);
	}

	private void CloseWindow()
	{
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		closeButton.onClick.RemoveListener(handler);
	}
}
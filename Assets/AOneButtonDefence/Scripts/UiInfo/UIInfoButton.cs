
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInfoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button Button;
    public float Timer = 0.5f;

    private GameObject panel;
    private InfoPanelScriptableObject panelData;
    private UIInfoPanel panelScript;
    private float _currentTime;
    private bool _timerOn = false;

    public void Initialize(InfoPanelScriptableObject panelData, UIInfoPanel infoPanel)
    {
        panel = infoPanel.gameObject;
        panelScript = infoPanel;
        this.panelData = panelData;
        panel.SetActive(false);
    }

    private void Update()
    {
        if (_timerOn)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= Timer)
            {

                panel.SetActive(true);
                panelScript.Initializator(panelData);
                _timerOn = false;
                _currentTime = 0;

            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _timerOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
        _timerOn = false;
        _currentTime = 0;
    }
}

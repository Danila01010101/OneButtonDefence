using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInfoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button Button;
    public float Timer = 1.2f;

    private GameObject panel;
    private BasicBuildingData panelData;
    private UIInfoPanel panelScript;
    private float currentTime;
    private bool timerOn = false;

    public void Initialize(BasicBuildingData panelData, UIInfoPanel infoPanel)
    {
        panel = infoPanel.gameObject;
        panelScript = infoPanel;
        this.panelData = panelData;
        panel.SetActive(false);
    }

    private void Update()
    {
        if (timerOn)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= Timer)
            {

                panel.SetActive(true);
                panelScript.Initialize(panelData);
                timerOn = false;
                currentTime = 0;

            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        timerOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
        timerOn = false;
        currentTime = 0;
    }
}

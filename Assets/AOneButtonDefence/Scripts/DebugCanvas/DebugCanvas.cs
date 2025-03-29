using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    private float time = 0.5f;
    private float frames;
    private TextMeshProUGUI DebugText;

    private float spendTime;

    private float currentFPS;

    private void Start()
    {
        DebugText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        frames++;
        spendTime += Time.deltaTime;
        if (spendTime >= time) 
        {
            UpdateCanvas();
            spendTime -= time;
            frames = 0;
            
        }
    }

    private void UpdateCanvas()
    {
        currentFPS = frames / spendTime;
        DebugText.text = $"FPS: {currentFPS:F0}";
    }
}

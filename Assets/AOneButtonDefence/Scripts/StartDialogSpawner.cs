using System;
using UnityEngine;

public class StartDialogSpawner : MonoBehaviour
{
    [SerializeField] private Canvas startDialogCanvas;

    private Canvas spawnedDialogCanvas;
    private Canvas gamePlayCanvas;

    public static Action StartDialogEnded;

    public void Initialize(Canvas canvas)
    {
        gamePlayCanvas = canvas;
    }

    public void SpawnDialogCanvas() => spawnedDialogCanvas = Instantiate(startDialogCanvas);

    public void RemoveStartCanvas() 
    {
        Destroy(spawnedDialogCanvas.gameObject);
        StartDialogEnded?.Invoke();
    }
}
using UnityEngine;

public class DialogNPCSpawner : MonoBehaviour
{
    [Header("Спавн модельки нпс")]
    [SerializeField] private GameObject DialogueNPCPrefab;
    [SerializeField] private Vector2 viewportPosition;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float distanceFromCamera = 5f;

    private GameObject spawnedNPC;
    private Camera mainCamera;

    public void SpawnDialogNPC()
    {
        mainCamera = Camera.main;
        Vector3 spawnPosition = new Vector3(viewportPosition.x, viewportPosition.y, distanceFromCamera);
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(spawnPosition);
        spawnedNPC = Instantiate(DialogueNPCPrefab, worldPosition, Quaternion.Euler(rotation), mainCamera.transform);
    }

    public void DeleteNPC()
    {
        if (spawnedNPC != null)
        {
            Destroy(spawnedNPC);
        }
    }
}
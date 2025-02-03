using UnityEngine;

public class UIGameObjectShower : MonoBehaviour
{
    public static UIGameObjectShower Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform modelContainer;

    public Camera UICamera => uiCamera;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RenderPrefab(GameObject modelPrefab, Vector3 position, Quaternion? rotation = null)
    {
        foreach (Transform child in modelContainer)
        {
            Destroy(child.gameObject);
        }

        GameObject modelInstance = Instantiate(modelPrefab, modelContainer);
        modelInstance.transform.localPosition = position;
        modelInstance.transform.localRotation = rotation ?? Quaternion.identity;

        SetLayerRecursively(modelInstance, uiCamera.cullingMask);
    }
    
    
    public void TeleportToUICamera(GameObject worldObject, Camera mainCamera)
    {
        if (!worldObject || !mainCamera || !uiCamera)
        {
            Debug.LogError("Не заданы необходимые параметры: worldObject, mainCamera, uiCamera, uiTarget или uiCanvas.");
            return;
        }

        Vector3 localPositionToMainCamera = mainCamera.transform.InverseTransformPoint(worldObject.transform.position);
        Vector3 worldPositionRelativeToUICamera = uiCamera.transform.TransformPoint(localPositionToMainCamera);
        worldObject.transform.SetParent(modelContainer);
        worldObject.transform.position = worldPositionRelativeToUICamera;
        SetLayerRecursively(worldObject, uiCamera.cullingMask);
    }

    private void SetLayerRecursively(GameObject obj, LayerMask layerMask)
    {
        int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerMask);
        }
    }
}

using UnityEngine;

public class UIGameObjectShower : MonoBehaviour
{
    public static UIGameObjectShower Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform modelContainer;

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

    /// <summary>
    /// Отрисовать 3D модель в UI.
    /// </summary>
    /// <param name="modelPrefab">Префаб 3D модели для отображения.</param>
    /// <param name="position">Позиция модели относительно контейнера.</param>
    /// <param name="rotation">Поворот модели (необязательно).</param>
    public void RenderModel(GameObject modelPrefab, Vector3 position, Quaternion? rotation = null)
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
    
    /// <summary>
    /// Перемещает объект из мирового пространства Main Camera в пространство UI Camera.
    /// </summary>
    /// <param name="worldObject">Объект в мировом пространстве.</param>
    /// <param name="mainCamera">Камера, в которой находится объект.</param>
    public void TeleportToUICamera(GameObject worldObject, Camera mainCamera)
    {
        if (!worldObject || !mainCamera || !uiCamera)
        {
            Debug.LogError("Не заданы необходимые параметры: worldObject, mainCamera, uiCamera, uiTarget или uiCanvas.");
            return;
        }

        // Рассчитываем локальную позицию объекта относительно Main Camera
        Vector3 localPositionToMainCamera = mainCamera.transform.InverseTransformPoint(worldObject.transform.position);

        // Преобразуем локальную позицию в мировые координаты относительно UI камеры
        Vector3 worldPositionRelativeToUICamera = uiCamera.transform.TransformPoint(localPositionToMainCamera);

        // Устанавливаем объект в новую позицию
        worldObject.transform.SetParent(modelContainer);
        worldObject.transform.position = worldPositionRelativeToUICamera;

        // Устанавливаем объект на нужный слой, чтобы он отображался UI камерой
        SetLayerRecursively(worldObject, uiCamera.cullingMask);
    }

    /// <summary>
    /// Рекурсивно устанавливает слой объекту и всем его дочерним объектам.
    /// </summary>
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

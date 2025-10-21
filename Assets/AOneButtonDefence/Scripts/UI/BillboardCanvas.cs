using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Camera targetCamera;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }
    
    public void SetCamera(Camera camera) => targetCamera = camera;

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        transform.forward = targetCamera.transform.forward;
    }
}
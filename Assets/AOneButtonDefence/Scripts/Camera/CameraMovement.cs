using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private CameraData data;

    private void Start()
    {
        transform.position = data.StartCameraPosition;
    }

    private void Update()
    {
        
    }
}
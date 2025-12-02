using UnityEngine;

public class FogFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform targetCamera;
    [SerializeField] private float height = 0f;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 camPos = targetCamera.position;

        transform.position = new Vector3(
            camPos.x,
            height,
            camPos.z
        );
    }
}
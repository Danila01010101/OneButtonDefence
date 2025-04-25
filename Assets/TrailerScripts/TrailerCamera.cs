using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class TrailerCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;

    float yaw, pitch;

    void Update()
    {
        if (!Input.GetMouseButton(1)) return;

        yaw += Input.GetAxis("Mouse X") * lookSpeed;
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.E)) direction.y += 1;
        if (Input.GetKey(KeyCode.Q)) direction.y -= 1;

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.Self);
    }
}

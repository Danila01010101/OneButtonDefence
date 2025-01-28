using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithRBM : MonoBehaviour
{
    public float Speed = 5f;
    public Transform GameObject;
    void Update()
    {
        if (Input.GetMouseButton(1))
        {

            GameObject.localEulerAngles = new Vector3(GameObject.localEulerAngles.x, GameObject.localEulerAngles.y + Input.GetAxis("Mouse X") * Time.deltaTime * Speed, GameObject.localEulerAngles.z);
        }
    }
}

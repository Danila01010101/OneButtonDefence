using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimationCitadel : MonoBehaviour
{
    public GameObject Core;
    public GameObject CenterCore;
    public GameObject BothCore;
    public float RotationSpeedX = 45f; // �������� �������� �� ��� X
    public float RotationSpeedY = 45f; // �������� �������� �� ��� Y
    public float RotationSpeedZ = 45f; // �������� �������� �� ��� Z
    public float MovingDistance = 0.1f; // ������������ ��������� �����������
    public float MovingSpeed = 1f; // �������� �����������
    private Vector3 startBothCorePosition; // ��������� ������� BothCore
    private float journeyProgress = 0f; // �������� ��������

    private void Start()
    {
        startBothCorePosition = BothCore.transform.position; // ���������� ��������� �������
    }

    void Update()
    {
        // ��������� ������� ��� �������� �����
        float rotationX = RotationSpeedX * Time.deltaTime;
        float rotationY = RotationSpeedY * Time.deltaTime;
        float rotationZ = RotationSpeedZ * Time.deltaTime;

        // ��������� �������� � �������
        Core.transform.Rotate(rotationX, rotationY, rotationZ);
        CenterCore.transform.Rotate(-rotationX, -rotationY, -rotationZ);

        // ����������� �������� ��������
        journeyProgress += MovingSpeed * Time.deltaTime;

        // ������������� �������� ��� �������� (�� -1 �� 1)
        float t = Mathf.PingPong(journeyProgress, MovingDistance * 2) - MovingDistance; // ��������� �� -MovingDistance �� MovingDistance

        // ���������� SmoothStep ��� �������� ��������
        float smoothY = Mathf.SmoothStep(-MovingDistance, MovingDistance, (t + MovingDistance) / (MovingDistance * 2));

        // ��������� ������� BothCore
        BothCore.transform.position = new Vector3(BothCore.transform.position.x, startBothCorePosition.y + smoothY, BothCore.transform.position.z);
    }
}
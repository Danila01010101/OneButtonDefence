using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimationCitadel : MonoBehaviour
{
    public GameObject Core;
    public GameObject CenterCore;
    public GameObject BothCore;
    public float RotationSpeedX = 45f; // Скорость вращения по оси X
    public float RotationSpeedY = 45f; // Скорость вращения по оси Y
    public float RotationSpeedZ = 45f; // Скорость вращения по оси Z
    public float MovingDistance = 0.1f; // Максимальная дистанция перемещения
    public float MovingSpeed = 1f; // Скорость перемещения
    private Vector3 startBothCorePosition; // Начальная позиция BothCore
    private float journeyProgress = 0f; // Прогресс движения

    private void Start()
    {
        startBothCorePosition = BothCore.transform.position; // Запоминаем начальную позицию
    }

    void Update()
    {
        // Вычисляем поворот для текущего кадра
        float rotationX = RotationSpeedX * Time.deltaTime;
        float rotationY = RotationSpeedY * Time.deltaTime;
        float rotationZ = RotationSpeedZ * Time.deltaTime;

        // Применяем вращение к объекту
        Core.transform.Rotate(rotationX, rotationY, rotationZ);
        CenterCore.transform.Rotate(-rotationX, -rotationY, -rotationZ);

        // Увеличиваем прогресс движения
        journeyProgress += MovingSpeed * Time.deltaTime;

        // Периодическое значение для движения (от -1 до 1)
        float t = Mathf.PingPong(journeyProgress, MovingDistance * 2) - MovingDistance; // Двигается от -MovingDistance до MovingDistance

        // Используем SmoothStep для плавного движения
        float smoothY = Mathf.SmoothStep(-MovingDistance, MovingDistance, (t + MovingDistance) / (MovingDistance * 2));

        // Обновляем позицию BothCore
        BothCore.transform.position = new Vector3(BothCore.transform.position.x, startBothCorePosition.y + smoothY, BothCore.transform.position.z);
    }
}
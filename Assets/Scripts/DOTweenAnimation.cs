using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimation : MonoBehaviour
{
    public GameObject Human;
    public Transform SpawnPoint;
    public List<GameObject> DoneHarvestPoints = new List<GameObject>();
    public List<GameObject> GrowingPlants = new List<GameObject>();
    public float WalkDuration;
    public float HarvestTime;
    public float GrowTime;

    private GameObject worker;

    void Start()
    {
        worker = Instantiate(Human, SpawnPoint.transform.position, Quaternion.identity);
        StartCoroutine(WorkerRoutine(worker));
    }

    IEnumerator WorkerRoutine(GameObject worker)
    {
        while (true)
        {
            if (DoneHarvestPoints.Count == 0)
            {
                // Ждем, пока в DoneHarvestPoints появятся элементы
                yield return new WaitForSeconds(1f);
                continue;
            }

            // Выбираем случайный "собранный" пункт
            int randpoint = Random.Range(0, DoneHarvestPoints.Count);
            var point = DoneHarvestPoints[randpoint];

            // Перемещаем работника к пункту
            worker.transform.DOMove(point.transform.position, WalkDuration);
            worker.transform.DOShakeRotation(WalkDuration);
            yield return new WaitForSeconds(WalkDuration);

            // Время на сбор урожая
            yield return new WaitForSeconds(HarvestTime);

            // Складываем растение, добавляем в список растущих растений
            point.transform.localScale = Vector3.zero;

            if (!GrowingPlants.Contains(point))  // Проверяем, добавлен ли уже элемент
            {
                GrowingPlants.Add(point);
            }
            DoneHarvestPoints.Remove(point);

            // Начинаем процесс роста растения
            StartCoroutine(Growing());

            // Возвращаем работника на исходную точку
            worker.transform.DOMove(SpawnPoint.position, WalkDuration);
            worker.transform.DOShakeRotation(WalkDuration);
            yield return new WaitForSeconds(WalkDuration);
        }
    }

    IEnumerator Growing()
    {
        for (int i = GrowingPlants.Count - 1; i >= 0; i--)
        {
            var plants = GrowingPlants[i];

            // Масштабируем растение
            plants.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), GrowTime);
            yield return new WaitForSeconds(GrowTime);

            // Проверяем, чтобы не добавлять элемент дважды
            if (!DoneHarvestPoints.Contains(plants))
            {
                DoneHarvestPoints.Add(plants);
            }
            GrowingPlants.RemoveAt(i);
        }
    }
}
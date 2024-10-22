using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimationCitadel : MonoBehaviour
{
    public GameObject Core;
    public GameObject CenterCore;
    public GameObject BothCore;
    public GameObject Gnome;
    public List<Transform> SpawnPositions;
    public List<Transform> EndPositions;
    public float RotationSpeedX = 45f; // Скорость вращения по оси X
    public float RotationSpeedY = 45f; // Скорость вращения по оси Y
    public float RotationSpeedZ = 45f; // Скорость вращения по оси Z
    public float MovingDistance = 0.1f; // Максимальная дистанция перемещения
    public float MovingSpeed = 1f; // Скорость перемещения
    public float SpawnDelay;
    public float WalkDuration;
    public float FadeDuration;
    public float PoklonTime;
    public float Poklon;
    public float Wait;

    private ChangeMaterial changeMaterial;
    private Vector3 startBothCorePosition; // Начальная позиция BothCore
    private float journeyProgress = 0f; // Прогресс движения

    [SerializeField]private int ModelChildIndex = 0;
    [SerializeField] private Color endvalue;
    [SerializeField] private Color startvalue;

    private void Start()
    {
        changeMaterial = gameObject.GetComponent<ChangeMaterial>();
        startBothCorePosition = BothCore.transform.position; // Запоминаем начальную позицию
        StartCoroutine(StartSpawnGnomes());
    }

    // Спавняться на старте
    private IEnumerator StartSpawnGnomes()
    {
        for (int i = 0; i < SpawnPositions.Count; i++)
        {
            GameObject gnome = Instantiate(Gnome, SpawnPositions[i].position, Quaternion.identity);
            gnome.transform.GetChild(1).gameObject.SetActive(false);
            Material material = gnome.transform.GetChild(ModelChildIndex).GetChild(ModelChildIndex).gameObject.GetComponent<Renderer>().material;
            material.color = startvalue;
            changeMaterial.SetTransparent(material);
            material.DOColor(endvalue, FadeDuration);
            yield return new WaitForSeconds(FadeDuration);
            changeMaterial.SetOpaque(material);
            yield return new WaitForSeconds(SpawnDelay);
            StartCoroutine(AnimationGnomes(gnome, SpawnPositions[i], EndPositions[i]));
        }
    }

    // Анимация движения и поклона гнома
    private IEnumerator AnimationGnomes(GameObject gnome, Transform startposition, Transform endposition)
    {
        // Вращение к конечной точке только по оси Y
        Vector3 directionToTarget = (endposition.position - gnome.transform.position).normalized;
        float targetYRotation = Quaternion.LookRotation(directionToTarget).eulerAngles.y;
        gnome.transform.DORotate(new Vector3(0, targetYRotation, 0), 0.5f);

        // Движение гнома
        Material material = gnome.transform.GetChild(ModelChildIndex).GetChild(ModelChildIndex).gameObject.GetComponent<Renderer>().material;
        gnome.transform.DOMove(endposition.position, WalkDuration);
        yield return new WaitForSeconds(WalkDuration);

        // Поклон перед возвратом
        gnome.transform.DOLocalRotate(new Vector3(Poklon, gnome.transform.localEulerAngles.y, gnome.transform.localEulerAngles.z), PoklonTime).OnComplete(() =>
        {
            gnome.transform.DOLocalRotate(new Vector3(0, gnome.transform.localEulerAngles.y, gnome.transform.localEulerAngles.z), PoklonTime);  // Возвращаем в исходное положение после поклона
        });
        yield return new WaitForSeconds(PoklonTime * 2 + Wait);

        // Вращение к стартовой позиции только по оси Y
        Vector3 directionToStart = (startposition.position - gnome.transform.position).normalized;
        float returnYRotation = Quaternion.LookRotation(directionToStart).eulerAngles.y;
        gnome.transform.DORotate(new Vector3(0, returnYRotation, 0), 0.5f);

        // Возвращение к стартовой позиции
        gnome.transform.DOMove(startposition.position, WalkDuration);
        yield return new WaitForSeconds(WalkDuration);

        // Исчезновение
        changeMaterial.SetTransparent(material);
        material.DOColor(startvalue, FadeDuration);
        yield return new WaitForSeconds(FadeDuration);
        Destroy(gnome);

        // Спавн нового гнома
        StartCoroutine(SpawnGnome(startposition, endposition));
    }

    private IEnumerator SpawnGnome(Transform startposition, Transform endposition)
    {
        GameObject gnome = Instantiate(Gnome, startposition.position, Quaternion.identity);
        gnome.transform.GetChild(1).gameObject.SetActive(false);
        Material material = gnome.transform.GetChild(ModelChildIndex).GetChild(ModelChildIndex).gameObject.GetComponent<Renderer>().material;
        material.color = startvalue;
        changeMaterial.SetTransparent(material);
        material.DOColor(endvalue, FadeDuration);
        yield return new WaitForSeconds(FadeDuration);
        changeMaterial.SetOpaque(material);
        StartCoroutine(AnimationGnomes(gnome, startposition, endposition));
    }

    private void Update()
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
        float t = Mathf.PingPong(journeyProgress, MovingDistance * 2) - MovingDistance;

        // Плавное движение
        float smoothY = Mathf.SmoothStep(-MovingDistance, MovingDistance, (t + MovingDistance) / (MovingDistance * 2));

        // Обновляем позицию BothCore
        BothCore.transform.position = new Vector3(BothCore.transform.position.x, startBothCorePosition.y + smoothY, BothCore.transform.position.z);
    }
}
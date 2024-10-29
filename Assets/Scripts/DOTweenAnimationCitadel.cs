using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimationCitadel : MonoBehaviour, IAnimatable
{
    [SerializeField] private GameObject Core;
    [SerializeField] private GameObject CenterCore;
    [SerializeField] private GameObject BothCore;
    [SerializeField] private GameObject Gnome;
    [SerializeField] private List<Transform> SpawnPositions;
    [SerializeField] private List<Transform> EndPositions;
    [SerializeField] private float RotationSpeedX = 45f;
    [SerializeField] private float RotationSpeedY = 45f;
    [SerializeField] private float RotationSpeedZ = 45f;
    [SerializeField] private float MovingDistance = 0.1f;
    [SerializeField] private float MovingSpeed = 1f;
    [SerializeField] private float SpawnDelay;
    [SerializeField] private float WalkDuration;
    [SerializeField] private float FadeDuration;
    [SerializeField] private float PoklonTime;
    [SerializeField] private float Poklon;
    [SerializeField] private float Wait;
    [SerializeField] private int ModelChildIndex = 0;
    [SerializeField] private Color endvalue;
    [SerializeField] private Color startvalue;

    private Coroutine currentAnimation;
    private ChangeMaterial changeMaterial;
    private Vector3 startBothCorePosition;
    private float journeyProgress = 0f;

    private void Start()
    {
        changeMaterial = gameObject.GetComponent<ChangeMaterial>();
        startBothCorePosition = BothCore.transform.position;
    }

    public void StartAnimation() => currentAnimation = StartCoroutine(StartSpawnGnomes());

    public void InteruptAnimation()
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
    }

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

    private IEnumerator AnimationGnomes(GameObject gnome, Transform startposition, Transform endposition)
    {
        Vector3 directionToTarget = (endposition.position - gnome.transform.position).normalized;
        float targetYRotation = Quaternion.LookRotation(directionToTarget).eulerAngles.y;
        gnome.transform.DORotate(new Vector3(0, targetYRotation, 0), 0.5f);

        Material material = gnome.transform.GetChild(ModelChildIndex).GetChild(ModelChildIndex).gameObject.GetComponent<Renderer>().material;
        StartGnomeMovementAnimation(gnome, endposition);

        yield return new WaitForSeconds(WalkDuration);

        gnome.transform.DOLocalRotate(new Vector3(Poklon, gnome.transform.localEulerAngles.y, gnome.transform.localEulerAngles.z), PoklonTime).OnComplete(() =>
        {
            gnome.transform.DOLocalRotate(new Vector3(0, gnome.transform.localEulerAngles.y, gnome.transform.localEulerAngles.z), PoklonTime);
        });

        yield return new WaitForSeconds(PoklonTime * 2 + Wait);
        Vector3 directionToStart = (startposition.position - gnome.transform.position).normalized;
        float returnYRotation = Quaternion.LookRotation(directionToStart).eulerAngles.y;
        gnome.transform.DORotate(new Vector3(0, returnYRotation, 0), 0.5f);
        gnome.transform.DOMove(startposition.position, WalkDuration);
        yield return new WaitForSeconds(WalkDuration);
        changeMaterial.SetTransparent(material);
        material.DOColor(startvalue, FadeDuration);
        yield return new WaitForSeconds(FadeDuration);
        Destroy(gnome);
        StartCoroutine(SpawnGnome(startposition, endposition));
    }

    private void StartGnomeMovementAnimation(GameObject gnome, Transform endPosition) => gnome.transform.DOMove(endPosition.position, WalkDuration);

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
        float rotationX = RotationSpeedX * Time.deltaTime;
        float rotationY = RotationSpeedY * Time.deltaTime;
        float rotationZ = RotationSpeedZ * Time.deltaTime;

        Core.transform.Rotate(rotationX, rotationY, rotationZ);
        CenterCore.transform.Rotate(-rotationX, -rotationY, -rotationZ);

        journeyProgress += MovingSpeed * Time.deltaTime;

        float t = Mathf.PingPong(journeyProgress, MovingDistance * 2) - MovingDistance;

        float smoothY = Mathf.SmoothStep(-MovingDistance, MovingDistance, (t + MovingDistance) / (MovingDistance * 2));

        BothCore.transform.position = new Vector3(BothCore.transform.position.x, startBothCorePosition.y + smoothY, BothCore.transform.position.z);
    }
}
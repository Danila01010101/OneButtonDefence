using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Paths
{
    public List<Transform> Path;
}

public class DOTweenAnimationMillitary : MonoBehaviour, IAnimatable
{
    [SerializeField] private GameObject WarriorPrefab;
    [SerializeField] private List<Paths> Path;
    [SerializeField] private float SpawnDelay;
    [SerializeField] private float WalkDelay;
    [SerializeField] private float SleepTime;
    [SerializeField] private int SpawnCount;
    [SerializeField] private float TrainingDelay;
    [SerializeField] private float AttackDuration;
    [SerializeField] private float RotateDuration;
    [SerializeField] private float Z;
    [SerializeField] private float ShakeStrenght;

    private List<GameObject> gnomes = new List<GameObject>();
    private Coroutine currentAnimation;

    public void StartAnimation() => currentAnimation = StartCoroutine(SpawnGnomes());

    public void InteruptAnimation() 
    {
        if (currentAnimation != null) 
            StopCoroutine(currentAnimation);
    }

    private IEnumerator SpawnGnomes()
    {
        for (int i = 0; i < Path.Count; i++)
        {
            GameObject gnome = Instantiate(WarriorPrefab, new Vector3(Path[i].Path[0].position.x, Path[i].Path[0].position.y, Path[i].Path[0].position.z + Z), Quaternion.identity);
            gnomes.Add(gnome);
            StartCoroutine(AnimationWarriors(gnome));
            yield return new WaitForSeconds(SpawnDelay);
        }
    }

    private IEnumerator AnimationWarriors(GameObject gnome)
    {
        while (true)
        {
            int index = gnomes.IndexOf(gnome);

            for (int i = 0; i < Path[index].Path.Count; i++)
            {
                gnome.transform.DOLookAt(Path[index].Path[i].position, RotateDuration);
                yield return new WaitForSeconds(RotateDuration);
                gnome.transform.DOMove(Path[index].Path[i].position, WalkDelay);
                gnome.transform.DOShakeRotation(WalkDelay);
                yield return new WaitForSeconds(WalkDelay);
            }

            int attackCount = (int)(TrainingDelay/AttackDuration);

            for (int i = 0; i < attackCount; i++)
            {
                gnome.transform.DOShakeRotation(AttackDuration, ShakeStrenght);
                yield return new WaitForSeconds(AttackDuration);
            }

            for (int i = Path[index].Path.Count - 1; i >= 0; i--)
            {
                gnome.transform.DOLookAt(Path[index].Path[i].position, RotateDuration);
                yield return new WaitForSeconds(RotateDuration);
                gnome.transform.DOMove(Path[index].Path[i].position, WalkDelay);
                gnome.transform.DOShakeRotation(WalkDelay);
                yield return new WaitForSeconds(WalkDelay);
            }

            yield return new WaitForSeconds(SleepTime);
        }
    }
}

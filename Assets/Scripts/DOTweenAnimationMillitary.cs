using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Paths
{
    public List<Transform> Path;
}

public class DOTweenAnimationMillitary : MonoBehaviour
{
    public GameObject WarriorPrefab;
    public List<Paths> Path;
    public float SpawnDelay;
    public float WalkDelay;
    public float SleepTime;
    public int SpawnCount;
    public float TrainingDelay;
    public float AttackDuration;
    public float RotateDuration;
    public float Z;
    public float ShakeStrenght;

    private List<GameObject> gnomes = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnGnomes());
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
            //Тут ошибка. Должны точки по порядку идти
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
                // Вращение во время атак
                gnome.transform.DOShakeRotation(AttackDuration, ShakeStrenght);
                yield return new WaitForSeconds(AttackDuration); // Ждем до следующего кадра
            }

            //и тут соответчтвенно
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

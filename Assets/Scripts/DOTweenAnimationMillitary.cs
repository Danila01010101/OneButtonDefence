using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAnimationMillitary : MonoBehaviour
{
    public GameObject WarriorPrefab;
    public List<Transform> SpawnPositions;
    public List<Transform> CenterPathsPoints;
    public List<Transform> EndPositions;

    private void Start()
    {

    }
    private IEnumerator AnimationWarriors(GameObject gnome, Transform startposition, Transform centerpathspoints, Transform endposition)
    {

        yield return null;
    }
}

using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GnomeWithFistables : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("Animation");
    }

    private IEnumerator Animation()
    {
        
        yield return null;
    }
}

using System.Collections;
using UnityEngine;

public class WaveCounterInitializer : MonoBehaviour, IGameComponentInitializer
{
    public IEnumerator Initialize()
    {
        new GameObject("WaveCounter").AddComponent<WaveCounter>().transform.SetParent(transform);
        yield return null;
    }
}
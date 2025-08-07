using System.Collections;
using UnityEngine;

public class UIObjectShowerInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private UIGameObjectShower uiGameObjectShowerPrefab;

    public IEnumerator Initialize()
    {
        Instantiate(uiGameObjectShowerPrefab, Vector3.up * 100, Quaternion.identity);
        yield return null;
    }
}
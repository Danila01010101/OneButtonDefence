using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInitializer : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> initializerBehaviours;

    private List<IGameComponentInitializer> _initializers = new();

    private void Awake()
    {
        foreach (var behaviour in initializerBehaviours)
        {
            if (behaviour is IGameComponentInitializer initializer)
                _initializers.Add(initializer);
        }

        StartCoroutine(InitializeAll());
    }

    private IEnumerator InitializeAll()
    {
        foreach (var initializer in _initializers)
            yield return initializer.Initialize();

        GameInitializer.GameInitialized?.Invoke();
    }
}
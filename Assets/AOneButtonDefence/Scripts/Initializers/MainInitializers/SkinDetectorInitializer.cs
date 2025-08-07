using System.Collections;
using UnityEngine;

public class SkinDetectorInitializer : MonoBehaviour, IGameComponentInitializer
{
    public static SkinChangeDetector Detector { get; private set; }

    public IEnumerator Initialize()
    {
        Detector = new SkinChangeDetector();
        yield return null;
    }

    private void OnDestroy()
    {
        Detector?.Unsubscribe();
    }
}
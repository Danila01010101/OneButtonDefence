using UnityEngine;

public class DisableOnDesktopSimple : MonoBehaviour
{
    private void Start()
    {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        gameObject.SetActive(false);
#endif
    }
}
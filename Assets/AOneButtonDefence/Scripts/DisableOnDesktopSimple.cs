using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class DisableOnDesktopWebGL : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool IsMobilePlatform();
#endif

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        bool isMobileWebGL = IsMobilePlatform();

        if (!isMobileWebGL)
        {
            gameObject.SetActive(false);
        }
#endif
    }
}
using System.Runtime.InteropServices;
using UnityEngine;

public class DisableOnDesktopSimple : MonoBehaviour
{
    [SerializeField] private bool disableOnMobileWebGL = true;
    [SerializeField] private bool disableOnDesktop = true;
    
    // JS функция для получения User Agent (только для WebGL)
    [DllImport("__Internal")]
    private static extern string GetUserAgent();
    
    private void Start()
    {
        bool shouldDisable = false;
        
        // Проверка для десктопных платформ
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        shouldDisable = disableOnDesktop;
#elif UNITY_WEBGL
        shouldDisable = CheckIfShouldDisableInWebGL();
#endif
        
        if (shouldDisable)
        {
            gameObject.SetActive(false);
        }
    }
    
    private bool CheckIfShouldDisableInWebGL()
    {
        // Если не нужно отключать на мобильном WebGL, возвращаем false
        if (!disableOnMobileWebGL) return false;
        
        try
        {
            // Пытаемся получить User Agent через JS
            string userAgent = GetUserAgent();
            return IsMobileUserAgent(userAgent);
        }
        catch
        {
            // Если не удалось, используем fallback методы
            return CheckMobileFallback();
        }
    }
    
    private bool IsMobileUserAgent(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
            return false;
            
        string ua = userAgent.ToLower();
        
        // Проверяем мобильные ключевые слова в User Agent
        return ua.Contains("mobile") || 
               ua.Contains("android") || 
               ua.Contains("iphone") || 
               ua.Contains("ipad") || 
               ua.Contains("ipod");
    }
    
    private bool CheckMobileFallback()
    {
        // Fallback метод через Screen
        float screenRatio = (float)Screen.width / Screen.height;
        
        // Мобильные устройства обычно имеют соотношение сторон близкое к портретному
        bool isLikelyMobile = screenRatio < 0.75f || screenRatio > 1.5f;
        
        // Дополнительная проверка по DPI
        bool highDPI = Screen.dpi > 250;
        
        return isLikelyMobile || highDPI;
    }
}
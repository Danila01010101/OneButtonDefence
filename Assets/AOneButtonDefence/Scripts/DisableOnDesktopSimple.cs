using UnityEngine;

public class DisableOnDesktopSimple : MonoBehaviour
{
    [SerializeField] private bool disableOnMobileWebGL = true;
    [SerializeField] private bool disableOnDesktop = true;
    
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
        
        return IsMobileDevice();
    }
    
    private bool IsMobileDevice()
    {
        // Метод 1: Проверка соотношения сторон (самый надежный для WebGL)
        float screenRatio = (float)Screen.width / Screen.height;
        
        // Мобильные устройства обычно имеют соотношение сторон:
        // - Портрет: ~9:16 = 0.5625
        // - Ландшафт: ~16:9 = 1.777
        bool isPortrait = screenRatio < 0.75f;  // Ширина < 75% высоты
        bool isLandscapeMobile = screenRatio > 1.5f && screenRatio < 2.0f;  // Типичное для мобильных
        
        // Метод 2: Размер экрана в пикселях
        bool smallScreen = Screen.width <= 768 || Screen.height <= 768;
        
        // Метод 3: Проверка touch support
        bool hasTouch = Input.touchSupported;
        
        // Метод 4: Плотность пикселей (только если доступно)
        bool highDPI = false;
        if (Screen.dpi > 0)
        {
            highDPI = Screen.dpi > 200; // Мобильные обычно > 200 DPI
        }
        
        // Логика определения:
        // Если устройство поддерживает тач И (имеет портретную ориентацию ИЛИ маленький экран)
        if (hasTouch)
            return true;
            
        // Если высокий DPI И портретная ориентация
        if (highDPI && isPortrait)
            return true;
            
        // Если специфичное для мобильных соотношение сторон
        if (isPortrait || isLandscapeMobile)
            return true;
            
        return false;
    }
}
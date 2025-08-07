using System.Collections;
using UnityEngine;

public class InputInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private GameData gameData;
    public static IInput Input { get; private set; }
    public static IDisableableInput DisableableInput { get; private set; }

    public IEnumerator Initialize()
    {
        if (Application.isMobilePlatform)
        {
            var mobileInput = new MobileInput(gameData.SwipeDeadZone, gameData.ClickMaxTime);
            Input = mobileInput;
            DisableableInput = mobileInput;
        }
        else
        {
            var desktopInput = new DesctopInput(gameData.SwipeDeadZone, gameData.ClickMaxTime);
            Input = desktopInput;
            DisableableInput = desktopInput;
        }

        yield return null;
    }
}
using UnityEngine;
using System.Collections;

public class InputInitializer : IGameInitializerStep
{
    private GameData _gameData;
    public IInput Input { get; private set; }
    public IDisableableInput DisableableInput { get; private set; }

    public InputInitializer(GameData data)
    {
        _gameData = data;
    }

    public IEnumerator Initialize()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var mobileInput = new MobileInput(_gameData.SwipeDeadZone, _gameData.ClickMaxTime);
            Input = mobileInput;
            DisableableInput = mobileInput;
        }
        else
        {
            var desktopInput = new DesctopInput(_gameData.SwipeDeadZone, _gameData.ClickMaxTime);
            Input = desktopInput;
            DisableableInput = desktopInput;
        }
        yield break;
    }
}
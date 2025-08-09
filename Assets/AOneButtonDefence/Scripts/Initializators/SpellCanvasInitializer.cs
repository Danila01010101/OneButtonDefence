using UnityEngine;
using System.Collections;

public class SpellCanvasInitializer : IGameInitializerStep
{
    private SpellCanvas _prefab;
    private IInput _input;
    private SpellCastData _data;
    public GameObject Instance { get; private set; }

    public SpellCanvasInitializer(SpellCanvas prefab, IInput input, SpellCastData data)
    {
        _prefab = prefab;
        _input = input;
        _data = data;
    }

    public IEnumerator Initialize()
    {
        var spellCanvasWindow = Object.Instantiate(_prefab);
        var spellCastScript = new SpellCast(_input, spellCanvasWindow, _data);
        spellCanvasWindow.gameObject.SetActive(false);
        Instance = spellCanvasWindow.gameObject;
        yield break;
    }
}
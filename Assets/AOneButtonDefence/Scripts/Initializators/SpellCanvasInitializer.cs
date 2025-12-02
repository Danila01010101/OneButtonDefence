using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class SpellCanvasInitializer : IGameInitializerStep, IDisposable
{
    private SpellCanvas _prefab;
    private IInput _input;
    private SpellCastData _data;
    private ICharacterStat _spellStat;
    private SpellCast spellCastScript;
    public GameObject Instance { get; private set; }

    public SpellCanvasInitializer(SpellCanvas prefab, IInput input, SpellCastData data)
    {
        _prefab = prefab;
        _input = input;
        _data = data;
    }

    public void SetSpellStat(ICharacterStat spellStat)
    {
        _spellStat = spellStat;
    }
    
    public IEnumerator Initialize()
    {
        var spellCanvasWindow = Object.Instantiate(_prefab);
        spellCastScript = new SpellCast(_input, spellCanvasWindow, _data, _spellStat);
        spellCanvasWindow.gameObject.SetActive(false);
        Instance = spellCanvasWindow.gameObject;
        yield break;
    }
    
    public void Dispose()
    {
        spellCastScript?.Dispose();
    }
}
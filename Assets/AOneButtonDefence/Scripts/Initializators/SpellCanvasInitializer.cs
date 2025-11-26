using UnityEngine;
using System.Collections;

public class SpellCanvasInitializer : IGameInitializerStep
{
    private SpellCanvas _prefab;
    private IInput _input;
    private SpellCastData _data;
    private ICharacterStat _spellStat;
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
        var spellCastScript = new SpellCast(_input, spellCanvasWindow, _data, _spellStat);
        spellCanvasWindow.gameObject.SetActive(false);
        Instance = spellCanvasWindow.gameObject;
        yield break;
    }
}
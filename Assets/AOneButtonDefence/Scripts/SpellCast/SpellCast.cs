using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
// Скрипт выдает спелы
//Скрипт кастует 
public class SpellCast
{
    private bool randomSpellMode = true;
    private bool active;

    private List<SpellStorage> spellStorage;
    private int currentChose;
    [Header("Graphic")]
    private SpellCanvas spellCanvas;

    private IInput input = new DesctopInput(0);
    private List<SpellData> randomSpell = new List<SpellData>();

    public SpellCast(IInput input, SpellCanvas spellCanvas, SpellCastData spellCastData)
    {
        this.spellCanvas = spellCanvas;
        this.spellStorage = spellCastData.SpellStorage;
        this.input = input;

        GameBattleState.BattleWon += () => spellCanvas.Active(false);
        GameBattleState.BattleLost += () => spellCanvas.Active(false);
        GameBattleState.BattleStarted += () => spellCanvas.Active(true);
        if (randomSpellMode == true)
        {
            input.Clicked += RandomModeCast;
            GameBattleState.BattleWon += AddNextSpell;
            InitilizeRandomMode();
        }
        else
        {
            input.Clicked += Cast;
        }
        spellCanvas.CurrentSpellButton.onClick.AddListener(onClick);
        Physics.queriesHitTriggers = false;
    }

    private void InitilizeRandomMode()
    {
        randomSpell.Add(spellStorage[Random.Range(0, spellStorage.Count)].Spell);
        randomSpell.Add(spellStorage[Random.Range(0, spellStorage.Count)].Spell);
        spellCanvas.ChangeUI(randomSpell[0].IconForUI, randomSpell[1].IconForUI);
        Debug.Log(randomSpell[0]);
        Debug.Log(randomSpell[1]);
    }

    private void Cast(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (active == true && spellStorage[currentChose].Count > 0 && Physics.Raycast(ray, out hit)) 
        {
            spellStorage[currentChose].Count--;
            GameObject spell = GameObject.Instantiate(spellStorage[currentChose].Spell.BaseMagicCircle, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
            spell.GetComponent<Spell>().Initialize(spellStorage[currentChose].Spell);
            active = false;
        }
        else 
        {
            active = false;
        }
    }

    private void RandomModeCast(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (randomSpell[0] == null) 
        {
            Debug.Log("Заклинание отсутсвует в первом слоте!");
            active = false;
            return;
        }
        if (active == true && Physics.Raycast(ray, out hit)) 
        {
            Debug.Log("Волшебство мутится");
            GameObject spell = GameObject.Instantiate(randomSpell[0].BaseMagicCircle, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
            spell.GetComponent<Spell>().Initialize(randomSpell[0]);
            randomSpell[0] = null;
            spellCanvas.CurrentSpell.enabled = false;
        }
    }

    public void AddNextSpell()
    {
        randomSpell[0] = randomSpell[1];
        randomSpell[1] = spellStorage[Random.Range(0, spellStorage.Count)].Spell;
        spellCanvas.CurrentSpell.enabled = true;
        spellCanvas.ChangeUI(randomSpell[0].IconForUI, randomSpell[1].IconForUI);
    }

    public void onClick()
    {
        active = true;
        Debug.Log("ТЫК");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Скрипт выдает спелы
//Скрипт кастует 
public class SpellCast : MonoBehaviour
{
    public bool RandomSpellMode;
    public bool Active;

    public List<SpellStorage> SpellStorage;
    public int CurrentChose;
    [Header("Graphic")]
    public Image CurrentSpell;
    public Image NextSpell;

    private IInput input = new DesctopInput(0);
    private List<SpellData> randomSpell = new List<SpellData>();

    public void Initialize(IInput input)
    {
        GameBattleState.BattleWon += AddNextSpell;
        this.input = input;
        if (RandomSpellMode == true)
        {
            input.Clicked += RandomModeCast;
            InitilizeRandomMode();
        }
        else
        {
            input.Clicked += Cast;
        }
        Physics.queriesHitTriggers = false;
    }

    private void InitilizeRandomMode()
    {
        randomSpell.Add(SpellStorage[Random.Range(0, SpellStorage.Count)].Spell);
        randomSpell.Add(SpellStorage[Random.Range(0, SpellStorage.Count)].Spell);
    }

    private void Update()
    {
        input.Update();
    }

    private void Cast(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Active == true && SpellStorage[CurrentChose].Count > 0 && Physics.Raycast(ray, out hit)) 
        {
            SpellStorage[CurrentChose].Count--;
            GameObject spell = Instantiate(SpellStorage[CurrentChose].Spell.BaseMagicCircle, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
            spell.GetComponent<Spell>().Initialize(SpellStorage[CurrentChose].Spell);
            Active = false;
        }
        else 
        {
            Active = false;
        }
    }

    private void RandomModeCast(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (randomSpell[0] == null) 
        {
            Debug.Log("Заклинание отсутсвует в первом слоте!");
            return;
        }
        if (Active == true && Physics.Raycast(ray, out hit)) 
        {
            GameObject spell = Instantiate(randomSpell[0].BaseMagicCircle, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
            spell.GetComponent<Spell>().Initialize(randomSpell[0]);
            randomSpell[0] = null;
        }
    }

    public void AddNextSpell()
    {
        randomSpell[0] = randomSpell[1];
        randomSpell[1] = SpellStorage[Random.Range(0, SpellStorage.Count)].Spell;
    }

    private void OnDestroy()
    {
        input.Clicked -= Cast;
    }
}

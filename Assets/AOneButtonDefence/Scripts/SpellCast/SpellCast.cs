using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Скрипт выдает спелы
//Скрипт кастует 
public class SpellCast : MonoBehaviour
{
    public List<SpellStorage> SpellStorage;
    public int CurrentChose;
    public bool Active;

    private IInput input = new DesctopInput(0);

    public void Initialize(IInput input)
    {
        this.input = input;
        input.Clicked += Cast;
        Physics.queriesHitTriggers = false;
    }
    private void Start()
    {
        Initialize(input);
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

    private void OnDestroy()
    {
        input.Clicked -= Cast;
    }
}

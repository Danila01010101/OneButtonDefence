using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Скрипт выдает спелы
//Скрипт кастует 
public class SpellCast : MonoBehaviour
{
    public SpellData Spell;
    private IInput input;

    public void Initialize(IInput input)
    {
        this.input = input;
        input.Clicked += Cast;
    }

    private void Cast(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) 
        {
            Instantiate(Spell.BaseMagicCircle, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
        }

    }

    private void OnDestroy()
    {
        input.Clicked -= Cast;
    }
}

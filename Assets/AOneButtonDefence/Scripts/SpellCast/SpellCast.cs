using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCast
{
    private List<SpellStorage> spellStorage;
    private LayerMask spellSurfaceLayer;
    private LayerMask damagableTargetLayer;
    private bool isBattleGoing;
    private bool isReloading;
    private float reloadDuration;
    private int currentChose;
    [Header("Graphic")]
    private SpellCanvas spellCanvas;

    private IInput input;
    private List<SpellData> randomSpell = new List<SpellData>();
    
    private bool CanCastSpell => isBattleGoing && isReloading == false;

    public SpellCast(IInput input, SpellCanvas spellCanvas, SpellCastData spellCastData)
    {
        damagableTargetLayer = spellCastData.spellTargetLayer;
        this.spellCanvas = spellCanvas;
        this.input = input;
        spellSurfaceLayer = spellCastData.spellCastLayerSurface;
        spellStorage = spellCastData.SpellStorage;
        reloadDuration = spellCastData.reloadDuration;
        
        if (true)
        {
            input.Clicked += RandomModeCast;
            GameBattleState.BattleWon += Disable;
            GameBattleState.BattleLost += Disable;
            GameBattleState.BattleStarted += Enable;
            InitilizeRandomMode();
        }
        
        Physics.queriesHitTriggers = false;
    }

    private void InitilizeRandomMode()
    {
        randomSpell.Add(spellStorage[Random.Range(0, spellStorage.Count)].Spell);
        randomSpell.Add(spellStorage[Random.Range(0, spellStorage.Count)].Spell);
        spellCanvas.ChangeUI(randomSpell[0].IconForUI, randomSpell[1].IconForUI, randomSpell[0].Background, randomSpell[1].MiniIcon, reloadDuration);
        AddNextSpell();
    }
    
    private void Enable() => isBattleGoing = true;
    
    private void Disable() => isBattleGoing = false;

    private void Cast(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (spellStorage[currentChose].Count > 0 && Physics.Raycast(ray, out hit)) 
        {
            spellStorage[currentChose].Count--;
            SpellCaster.Cast(spellStorage[currentChose].Spell.BaseMagicCircle, spellStorage[currentChose].Spell, new Vector3(hit.point.x, 1, hit.point.z), damagableTargetLayer);
        }
    }

    private void RandomModeCast(Vector2 position)
    {   
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        
        if (randomSpell[0] == null || CanCastSpell == false)
        {
            return;
        }
        
        if (Physics.Raycast(ray, out hit, spellSurfaceLayer)) 
        {
            GameObject spell = GameObject.Instantiate(randomSpell[0].BaseMagicCircle, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
            spell.GetComponent<Spell>().Initialize(randomSpell[0], damagableTargetLayer);
            randomSpell[0] = null;
            AddNextSpell();
            CoroutineStarter.Instance.StartCoroutine(Reload());
        }
    }
    
    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDuration);
        isReloading = false;
    }

    public void AddNextSpell()
    {
        randomSpell[0] = randomSpell[1];
        randomSpell[1] = spellStorage[Random.Range(0, spellStorage.Count)].Spell;
        spellCanvas.ChangeUI(randomSpell[0].IconForUI, randomSpell[1].IconForUI, randomSpell[0].Background, randomSpell[1].MiniIcon, reloadDuration);
    }
}

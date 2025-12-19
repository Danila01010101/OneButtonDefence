using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class SpellCast : IDisposable
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

    private ICharacterStat stat;

    private IInput input;
    private List<SpellData> randomSpell = new List<SpellData>();
    
    private bool CanCastSpell => isBattleGoing && isReloading == false;

    public SpellCast(IInput input, SpellCanvas spellCanvas, SpellCastData spellCastData, ICharacterStat spellStat)
    {
        stat = spellStat;
        damagableTargetLayer = spellCastData.spellTargetLayer;
        this.spellCanvas = spellCanvas;
        this.input = input;
        spellSurfaceLayer = spellCastData.spellCastLayerSurface;
        spellStorage = spellCastData.SpellStorage;
        reloadDuration = spellCastData.reloadDuration;
        
        input.Clicked += RandomModeCast;
        GameBattleState.BattleWon += Disable;
        GameBattleState.BattleLost += Disable;
        GameBattleState.BattleStarted += Enable;
        BossFightBattleState.BattleStarted += Enable;
        InitilizeRandomMode();
    }

    private void InitilizeRandomMode()
    {
        randomSpell.Add(spellStorage[Random.Range(0, spellStorage.Count)].Spell);
        randomSpell.Add(spellStorage[Random.Range(0, spellStorage.Count)].Spell);
        spellCanvas.ChangeUI(randomSpell[0].IconForUI, randomSpell[1].IconForUI, randomSpell[0].Background, randomSpell[1].MiniIcon, reloadDuration);
        AddNextSpell();
        spellCanvas.SetIcon(randomSpell[0].IconForUI, randomSpell[1].IconForUI, randomSpell[0].Background, randomSpell[1].MiniIcon);
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
            SpellCaster.Cast(spellStorage[currentChose].Spell.BaseMagicCircle, spellStorage[currentChose].Spell, new Vector3(hit.point.x, 1, hit.point.z), damagableTargetLayer, stat.Value);
        }
    }

    private void RandomModeCast(Vector2 position)
    {
    #if UNITY_EDITOR
        Debug.Log("SpellCast: Click received");
    #endif

        if (!CanCastSpell)
        {
    #if UNITY_EDITOR
            Debug.LogWarning(
                $"SpellCast blocked: CanCastSpell = false " +
                $"(Battle: {isBattleGoing}, Reloading: {isReloading})"
            );
    #endif
            return;
        }

        if (IsClickOnUI())
        {
    #if UNITY_EDITOR
            Debug.LogWarning("SpellCast blocked: click is over UI");
    #endif
            return;
        }

        if (randomSpell[0] == null)
        {
    #if UNITY_EDITOR
            Debug.LogWarning("SpellCast blocked: no spell loaded in slot 0");
    #endif
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
    #if UNITY_EDITOR
            Debug.LogError("SpellCast failed: Camera.main is null");
    #endif
            return;
        }

        Ray ray = cam.ScreenPointToRay(position);

    #if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.cyan, 1.5f);
    #endif

        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                Mathf.Infinity,
                spellSurfaceLayer,
                QueryTriggerInteraction.Ignore))
        {
    #if UNITY_EDITOR
            Debug.LogWarning(
                $"SpellCast blocked: Raycast did not hit spell surface. " +
                $"LayerMask: {spellSurfaceLayer.value}"
            );
    #endif
            return;
        }

    #if UNITY_EDITOR
        Debug.Log(
            $"SpellCast success: hit {hit.collider.name} " +
            $"(Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}) " +
            $"at distance {hit.distance}"
        );
    #endif

        Vector3 spawnPos = new Vector3(hit.point.x, 1.01f, hit.point.z);

        isReloading = true;

        GameObject spell = GameObject.Instantiate(
            randomSpell[0].BaseMagicCircle,
            spawnPos,
            Quaternion.identity);

        spell.GetComponent<Spell>().Initialize(
            randomSpell[0],
            damagableTargetLayer,
            stat.Value);

        randomSpell[0] = null;
        AddNextSpell();

        CoroutineStarter.Instance.StartCoroutine(Reload());
    }
    
    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDuration * ((100 - stat.Value) / 100));
        isReloading = false;
    }

    private bool IsClickOnUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count == 0)
            return false;

#if UNITY_EDITOR
        Debug.Log("UI blocks click. Hit elements:");
        foreach (var r in results)
        {
            var canvas = r.gameObject.GetComponentInParent<Canvas>();

            string canvasInfo = canvas != null
                ? $"Canvas: {canvas.name}, RenderMode: {canvas.renderMode}"
                : "Canvas: none";

            Debug.Log(
                $"UI Element: {r.gameObject.name} | " +
                $"Layer: {LayerMask.LayerToName(r.gameObject.layer)} | " +
                canvasInfo
            );
        }
#endif

        return true;
    }

    public void AddNextSpell()
    {
        randomSpell[0] = randomSpell[1];
        randomSpell[1] = spellStorage[Random.Range(0, spellStorage.Count)].Spell;
        spellCanvas.ChangeUI(randomSpell[0].IconForUI, randomSpell[1].IconForUI, randomSpell[0].Background, randomSpell[1].MiniIcon, reloadDuration);
    }

    public void Dispose()
    {
        input.Clicked -= RandomModeCast;
        GameBattleState.BattleWon -= Disable;
        GameBattleState.BattleLost -= Disable;
        GameBattleState.BattleStarted -= Enable;
        BossFightBattleState.BattleStarted -= Enable;
    }
}

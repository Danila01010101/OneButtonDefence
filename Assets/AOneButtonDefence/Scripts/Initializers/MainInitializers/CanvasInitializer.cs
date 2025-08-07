using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private GameplayCanvas gameplayCanvasPrefab;
    [SerializeField] private GameObject debugCanvasPrefab;
    [SerializeField] private SkinPanel shopSkinPanelPrefab;
    [SerializeField] private GameObject infoCanvasPrefab;
    [SerializeField] private GameData gameData;
    [SerializeField] private WorldGenerationData worldData;
    [SerializeField] private MusicData musicData;

    public static GameplayCanvas GameplayCanvas { get; private set; }

    public IEnumerator Initialize()
    {
        GameplayCanvas = Instantiate(gameplayCanvasPrefab);
        GameplayCanvas.Initialize(5, worldData.BuildingsData);
        GameplayCanvas.EnemiesCountIndicator.Initiallize(gameData.BattleWavesParameters);

        var debugCanvas = Instantiate(debugCanvasPrefab);
        if (!Debug.isDebugBuild)
            debugCanvas.SetActive(false);

        var shopPanel = Instantiate(shopSkinPanelPrefab, GameplayCanvas.transform);
        shopPanel.Initialize(InputInitializer.Input);

        var infoCanvas = Instantiate(infoCanvasPrefab);
        GameplayCanvas.DetectSettingsWindow(infoCanvas);
        infoCanvas.SetActive(false);

        var sources = new List<AudioSource>
        {
            MusicInitializer.BackgroundPlayer.GetSource()
        };
        sources.AddRange(MusicInitializer.UpgradeEffectPlayer.GetSources());

        GameplayCanvas.SoundSettings.Initialize(sources, musicData.StartValue);

        yield return null;
    }
}
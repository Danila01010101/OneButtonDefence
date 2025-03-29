using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

public class GnomeWithFistablesSpawn : MonoBehaviour
{
    
    [SerializeField]private int minTimeBetweenSpawns;
    [SerializeField]private int maxTimeBetweenSpawns;
    [SerializeField]private GnomeWithFistables gnomeWithFistablesPrefab;
    [SerializeField]private RectTransform endRectForGnome;
    [SerializeField]private RectTransform GnomeRectSpawn;
    [SerializeField]private Canvas GameplayCanvas;
    [SerializeField]private bool IsDebug;
    
    private float timer;
    private float timeBetweenSpawns;
    
    private void Start()
    {
        RandomTimeBetweenSpawns();
    }
    
    private void Update()
    {
        if (UpgradeState.IsTimerWork || IsDebug)
        {
            timer += Time.deltaTime;

            if (timer >= timeBetweenSpawns)
            {
                RandomTimeBetweenSpawns();
                SpawnGnome();
            }
        }
    }

    private void RandomTimeBetweenSpawns()
    { 
        timer = 0;
        timeBetweenSpawns = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }

    private void SpawnGnome()
    {
        GnomeWithFistables gnome = Instantiate(gnomeWithFistablesPrefab, GameplayCanvas.transform);
        gnome.gameObject.GetComponent<RectTransform>().anchoredPosition = GnomeRectSpawn.anchoredPosition;
        gnome.Initialize(endRectForGnome);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class GnomeWithFistablesSpawn : MonoBehaviour
{
    
    [SerializeField]private int minTimeBetweenSpawns;
    [SerializeField]private int maxTimeBetweenSpawns;
    [SerializeField]private Image GnomeImage;
    
    private float timer;
    private float timeBetweenSpawns;
    
    public RectTransform GnomeRectSpawn;
    public Canvas GameplayCanvas;

    public bool IsDebug;
    
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
        Image gnome = Instantiate(GnomeImage, GameplayCanvas.transform);
        gnome.rectTransform.anchoredPosition = GnomeRectSpawn.anchoredPosition;
    }
}

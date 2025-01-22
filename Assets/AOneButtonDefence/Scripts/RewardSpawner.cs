using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardSpawner : MonoBehaviour, IEnemyDeathListener
{
    private Gem rewardObjectPrefab;
    private RectTransform uiTarget;
    private Canvas canvas;
    private Camera mainCamera;
    private bool isUpgradeStarted = false;

    public void Initialize(Gem rewardObjectPrefab, RectTransform uiTarget, Canvas canvas, Camera mainCamera)
    {
        this.rewardObjectPrefab = rewardObjectPrefab;
        this.uiTarget = uiTarget;
        this.canvas = canvas;
        this.mainCamera = mainCamera;
    }

    private void Start()
    {
        EnemyDeathManager.Instance.RegisterListener(this);
    }

    private void OnDestroy()
    {
        if (Application.loadedLevelName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            EnemyDeathManager.Instance.UnregisterListener(this);
        }
    }
    
    private void DetectUpgradeStateStart() => isUpgradeStarted = true;

    public void OnEnemyDeath(Vector3 position, int currencyAmount)
    {
        for (int i = 0; i < currencyAmount; i++)
        {
            SpawnReward(position);
        }
    }

    private void SpawnReward(Vector3 position)
    {
        isUpgradeStarted = false;
        IEnemyReward currencyObject = Instantiate(rewardObjectPrefab, position, rewardObjectPrefab.transform.rotation);
        Vector3 newPosition = currencyObject.GameObject.transform.position + 
                              new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        currencyObject.GameObject.transform.DOJump(newPosition, 0.2f, 3, 2f, true).SetLoops(1, LoopType.Yoyo)
            .OnComplete(() => StartCoroutine(StartAnimation(currencyObject)));
    }

    private IEnumerator StartAnimation(IEnemyReward collectableObject)
    {
        while (isUpgradeStarted == false)
        {
            yield return null;
        }
        
        collectableObject.FlyToUI(collectableObject.GameObject.transform.position, uiTarget, canvas, () =>
        {
            ResourcesCounter.Instance.Data.GemsAmount++;
            collectableObject.Destroy();
        });
    }

    private void OnEnable()
    {
        UpgradeState.UpgradeStateStarted += DetectUpgradeStateStart;
    }

    private void OnDisable()
    {
        UpgradeState.UpgradeStateStarted -= DetectUpgradeStateStart;
    }
}
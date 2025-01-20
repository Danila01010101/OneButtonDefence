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
        EnemyDeathManager.Instance.UnregisterListener(this);
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
        IEnemyReward currencyObject = Instantiate(rewardObjectPrefab, position, Quaternion.identity);
        Vector3 newPosition = currencyObject.GameObject.transform.position + 
                              new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
        currencyObject.GameObject.transform.DOJump(newPosition, 2.5f, 3, .25f, true).SetLoops(-1, LoopType.Yoyo)
            .OnComplete(() => StartCoroutine(StartAnimation(currencyObject)));
    }

    private IEnumerator StartAnimation(IEnemyReward currencyObject)
    {
        while (isUpgradeStarted == false)
        {
            yield return null;
        }
        
        FlyToUI flyScript = currencyObject.UIAnimator;
        flyScript.Initialize(currencyObject.GameObject.transform.position, uiTarget, mainCamera, canvas, () =>
        {
            ResourcesCounter.Instance.Data.GemsAmount++;
            currencyObject.Destroy();
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
using System.Collections;
using UnityEngine;

public class RewardSpawner : MonoBehaviour, IEnemyDeathListener
{
    private Gem rewardObjectPrefab;
    private RectTransform uiTarget;
    private ResourcesCounter resourcesCounter;
    private bool isUpgradeStarted = false;
    private RewardAnimationSettings animationSettings;

    public void Initialize(Gem rewardObjectPrefab, RectTransform uiTarget, 
        RewardAnimationSettings rewardAnimationSettings, ResourcesCounter counter)
    {
        this.rewardObjectPrefab = rewardObjectPrefab;
        this.uiTarget = uiTarget;
        animationSettings = rewardAnimationSettings;
        resourcesCounter = counter;
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
            SpawnReward(position + Vector3.up * 0.1f);
        }
    }

    private void SpawnReward(Vector3 position)
    {
        isUpgradeStarted = false;
        IEnemyReward currencyObject = Instantiate(rewardObjectPrefab, position, rewardObjectPrefab.transform.rotation);
        currencyObject.BounceAside(delegate { StartCoroutine(StartAnimation(currencyObject)); });
        //Vector3 newPosition = currencyObject.GameObject.transform.position + 
        //                           new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        //currencyObject.GameObject.transform.DOJump(newPosition, 0.2f, 3, 2f, true).SetLoops(1, LoopType.Yoyo)
        //    .OnComplete(() => StartCoroutine(StartAnimation(currencyObject)));
    }

    private IEnumerator StartAnimation(IEnemyReward collectableObject)
    {
        while (isUpgradeStarted == false)
        {
            yield return null;
        }
        
        collectableObject.FlyToUI(UIGameObjectShower.Instance.UICamera, uiTarget, animationSettings.Duration, animationSettings.endScale, () =>
        {
            resourcesCounter.Data.GemsAmount++;
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

    public struct RewardAnimationSettings
    {
        public readonly float Duration;
        public readonly float endScale;

        public RewardAnimationSettings(float duration, float endScale)
        {
            this.Duration = duration;
            this.endScale = endScale;
        }
    }
}
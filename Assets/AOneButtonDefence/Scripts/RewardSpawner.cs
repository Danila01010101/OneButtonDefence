using System.Collections;
using UnityEngine;

public class RewardSpawner : MonoBehaviour, IEnemyDeathListener
{
    private Gem rewardObjectPrefab;
    private RectTransform uiTarget;
    private bool isUpgradeStarted;
    private RewardAnimationSettings animationSettings;
    private ResourceData gemResource;

    public void Initialize(Gem rewardObjectPrefab, RectTransform uiTarget, 
        RewardAnimationSettings rewardAnimationSettings, ResourceData gemResource)
    {
        this.gemResource = gemResource;
        this.rewardObjectPrefab = rewardObjectPrefab;
        this.uiTarget = uiTarget;
        animationSettings = rewardAnimationSettings;
    }

    private void Start()
    {
        EnemyDeathManager.Instance.RegisterListener(this);
    }

    private void OnDestroy()
    {
        EnemyDeathManager.Instance.UnregisterListener(this);
    }
    
    private void DetectFightEnd() => isUpgradeStarted = true;

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
        
        collectableObject.FlyToUI(UIGameObjectShower.Instance.UICamera, uiTarget, animationSettings.Duration, animationSettings.EndScale, () =>
        {
            ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(gemResource, 1));
            collectableObject.Destroy();
        });
    }

    private void OnEnable()
    {
        GameBattleState.EnemiesDefeated += DetectFightEnd;
    }

    private void OnDisable()
    {
        GameBattleState.EnemiesDefeated -= DetectFightEnd;
    }

    public struct RewardAnimationSettings
    {
        public readonly float Duration;
        public readonly float EndScale;

        public RewardAnimationSettings(float duration, float endScale)
        {
            Duration = duration;
            EndScale = endScale;
        }
    }
}
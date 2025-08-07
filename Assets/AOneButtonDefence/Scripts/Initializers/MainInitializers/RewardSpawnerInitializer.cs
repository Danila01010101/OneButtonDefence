using System.Collections;
using UnityEngine;

public class RewardSpawnerInitializer : MonoBehaviour, IGameComponentInitializer
{
    [SerializeField] private GameData gameData;

    public IEnumerator Initialize()
    {
        var rewardSpawner = new GameObject("RewardSpawner").AddComponent<RewardSpawner>();
        rewardSpawner.transform.SetParent(transform);

        rewardSpawner.Initialize(
            gameData.EnemyRewardPrefab,
            GemsView.Instance.GemsTextTransform,
            new RewardSpawner.RewardAnimationSettings(1, 1),
            gameData.GemsResource
        );

        yield return null;
    }
}
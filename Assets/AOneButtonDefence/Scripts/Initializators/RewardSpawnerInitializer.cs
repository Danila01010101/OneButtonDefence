using UnityEngine;
using System.Collections;

public class RewardSpawnerInitializer : IGameInitializerStep
{
    private Transform _parent;
    private GameData _data;

    public RewardSpawnerInitializer(Transform parent, GameData data)
    {
        _parent = parent;
        _data = data;
    }

    public IEnumerator Initialize()
    {
        var spawner = new GameObject("RewardSpawner").AddComponent<RewardSpawner>();
        spawner.transform.SetParent(_parent);
        spawner.Initialize(_data.EnemyRewardPrefab, GemsView.Instance.GemsTextTransform, new RewardSpawner.RewardAnimationSettings(1, 1), _data.GemsResource);
        yield break;
    }
}
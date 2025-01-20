using System;
using UnityEngine;

[RequireComponent(typeof(FlyToUI))]
public class Gem : MonoBehaviour, IEnemyReward
{
    [SerializeField] private ParticleSystem summonEffect;
    private IEnemyReward enemyRewardImplementation;

    public FlyToUI UIAnimator { get; private set; }
    public GameObject GameObject => gameObject;

    private void Awake()
    {
        UIAnimator = GetComponent<FlyToUI>();
    }
    public void PlayEffect() => Instantiate(summonEffect, transform.position, Quaternion.identity, Camera.main.transform);
    public void Destroy() => Destroy(gameObject);
}
using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class AdsReviver : IDisposable
{
    private PlayerController playerController;
    private GameResourcesCounter gameResourcesCounter;
    private ReviveResourcesData reviveResourcesData;
    private Button reviveButton;
    private bool isBattleGoing;
    private string reviveRewardText = "Revive reward.";

    public static event Action RewardGranted;

    public AdsReviver(PlayerController playerController, GameResourcesCounter gameResourcesCounter, ReviveResourcesData reviveResourcesData)
    {
        this.playerController = playerController;
        this.gameResourcesCounter = gameResourcesCounter;
        this.reviveResourcesData = reviveResourcesData;
        
        GameBattleState.BattleStarted += DetectBattleStart;
        GameBattleState.BattleWon += DetectBattleEnd;
    }

    public void SubscribeButton(Button reviveButton)
    {
        this.reviveButton?.onClick.RemoveListener(Revive);
        YG2.onRewardAdv -= ActivateReviveReward;
        this.reviveButton = reviveButton;
        reviveButton.onClick.AddListener(ActivateReviveReward);
        YG2.onRewardAdv += ActivateReviveReward;
        reviveButton.interactable = true;
    }

    private void ActivateReviveReward()
    {
        YG2.RewardedAdvShow(reviveRewardText);
    }

    private void ActivateReviveReward(string id)
    {
        if (reviveRewardText != id)
            return;
        
        reviveButton.interactable = false;
        Revive();
    }

    private void Revive()
    {
        reviveButton.onClick.AddListener(Revive);

        foreach (var resourceToAdd in reviveResourcesData.ReviveResources)
        {
            ResourceAmount resource = new ResourceAmount(resourceToAdd.Resource, resourceToAdd.Amount);
            gameResourcesCounter.ChangeResourceAmount(resource);
        }
        
        Building[] buildings = GameObject.FindObjectsOfType<Building>();
        var warriorResourceAmount = new ResourceAmount(reviveResourcesData.WarriorsResource,
            reviveResourcesData.WarriorsPerBuildingBonus);

        foreach (var building in buildings)
        {
            if (building.UpgradeType == BasicBuildingData.Upgrades.MilitaryCamp)
                ResourceIncomeCounter.Instance.InstantResourceChange(warriorResourceAmount, building.ResourceSpawnPosition);
        }
        
        RewardGranted?.Invoke();
    }

    private void DetectBattleStart() => isBattleGoing = true;

    private void DetectBattleEnd() => isBattleGoing = false;
    
    public void Dispose()
    {
        reviveButton?.onClick.RemoveListener(ActivateReviveReward);
        GameBattleState.BattleStarted += DetectBattleStart;
        GameBattleState.BattleWon += DetectBattleEnd;
    }
}
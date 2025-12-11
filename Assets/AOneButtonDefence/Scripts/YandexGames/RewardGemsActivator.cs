using System;
using UnityEngine.UI;
using YG;

public class RewardGemsActivator : IDisposable
{
    private GameResourcesCounter resourceCounter;
    private SkinPanel skinPanel;
    private ResourceData gemsResource;
    private Button adsButton;
    private int rewardAmount;
    private bool isFirstSkinUnlocked;
    private string rewardGemsText = "Skin reward.";

    public static event Action FirstSkinRewardActivated;
    public static event Action SkinRewardActivated;

    public RewardGemsActivator(GameResourcesCounter resourceCounter, ResourceData gemsResource, int rewardAmount)
    {
        this.resourceCounter = resourceCounter;
        this.gemsResource = gemsResource;
        this.rewardAmount = rewardAmount;
        YG2.onRewardAdv += ActivateReward;
    }

    public void InitializeSkinPanel(SkinPanel skinPanel)
    {
        this.skinPanel = skinPanel;
        this.adsButton = skinPanel.RewardButton;
        adsButton.onClick.AddListener(ActivateReward);
    }

    private void ActivateReward()
    {
        YG2.RewardedAdvShow(rewardGemsText);
    }

    private void ActivateReward(string id)
    {
        if (rewardGemsText != id)
            return;
        
        if (isFirstSkinUnlocked)
        {
            resourceCounter.ChangeResourceAmount(new ResourceAmount(gemsResource, rewardAmount));
            SkinRewardActivated?.Invoke();
        }
        else
        {
            var currenSkinChoose = skinPanel.SkinList[skinPanel.CurrentChose].Cost;
            resourceCounter.ChangeResourceAmount(new ResourceAmount(gemsResource, currenSkinChoose));
            skinPanel.SetSkin();
            FirstSkinRewardActivated?.Invoke();
            SkinRewardActivated?.Invoke();
            isFirstSkinUnlocked = true;
        }
    }

    public void Dispose()
    {
        YG2.onRewardAdv -= ActivateReward;
        adsButton.onClick.RemoveListener(ActivateReward);
    }
}

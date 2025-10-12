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

    public static event Action FirstRewardActivated;

    public RewardGemsActivator(GameResourcesCounter resourceCounter, ResourceData gemsResource, int rewardAmount)
    {
        this.resourceCounter = resourceCounter;
        this.gemsResource = gemsResource;
        this.rewardAmount = rewardAmount;
        YG2.onRewardAdv += ActivateRevard;
    }

    public void InitializeSkinPanel(SkinPanel skinPanel)
    {
        this.skinPanel = skinPanel;
        this.adsButton = skinPanel.RewardButton;
        adsButton.onClick.AddListener(ActivateReward);
    }

    private void ActivateReward()
    {
        YG2.RewardedAdvShow("Skin reward.");
    }

    private void ActivateRevard(string id)
    {
        
        if (isFirstSkinUnlocked)
        {
            resourceCounter.ChangeResourceAmount(new ResourceAmount(gemsResource, rewardAmount));
        }
        else
        {
            var currenSkinChoose = skinPanel.SkinList[skinPanel.CurrentChose].Cost;
            resourceCounter.ChangeResourceAmount(new ResourceAmount(gemsResource, currenSkinChoose));
            skinPanel.SetSkin();
            FirstRewardActivated?.Invoke();
            isFirstSkinUnlocked = true;
        }
    }

    public void Dispose()
    {
        YG2.onRewardAdv -= ActivateRevard;
        adsButton.onClick.RemoveListener(ActivateReward);
    }
}

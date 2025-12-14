using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TradeDialogueSystem : DialogueSystem
{
    [Header("Trade Dialogue System Data")]
    [SerializeField] private TradeDialogueData tradeDialogueData;
    [Header("Trade Dialogue System UI")]
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private TradeResourceInfoWindow tradeInfoPanelPrefab;

    private Vector3 warriorsIncomePosition;
    private Transform infoPanelParent;
    private GameResourcesCounter resourcesCounter;

    public override void Initialize(AdsReviver adsReviver = null)
    {
        closeOnReplicFinish = false;
        Slider.gameObject.SetActive(false);
        SkipTime = Mathf.Infinity;
        agreeButton.interactable = false;
        agreeButton.onClick.AddListener(AcceptTrade);
        declineButton.interactable = false;
        declineButton.onClick.AddListener(DeclineTrade);
    }

    public void SetupTradeDialogueComponents(GameResourcesCounter resourcesCounter, TradeDialogueData tradeDialogueData, Transform infoPanelParent, Vector3 warriorsIncomePosition)
    {
        this.resourcesCounter = resourcesCounter;
        this.tradeDialogueData = tradeDialogueData;
        DialogueData = tradeDialogueData;
        this.infoPanelParent = infoPanelParent;
        this.warriorsIncomePosition = warriorsIncomePosition;
        base.Initialize();
    }

    protected override void ChangeReplica()
    {
        if (numReplic == DialogueData.Label[numLabel].Replic.Count - 1)
        {
            agreeButton.interactable = true;
            declineButton.interactable = true;
        }
        
        base.ChangeReplica();
    }

    private void AcceptTrade() => FinishTrade(tradeDialogueData.AcceptResources);

    private void DeclineTrade() => FinishTrade(tradeDialogueData.DeclineResources);

    private void FinishTrade(List<StartResourceAmount> resourceAmounts)
    {
        if (resourcesCounter == null)
            throw new Exception("No data resource counter for trade");
        
        foreach (var resource in resourceAmounts)
        {
            if (resource.Resource.Type == ResourceData.ResourceType.Warrior)
                ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(resource.Resource, resource.Amount, warriorsIncomePosition));
            else
            {
                ResourceIncomeCounter.Instance.InstantResourceChange(new ResourceAmount(resource.Resource, resource.Amount));
            }
        }

        TradeResourceInfoWindow infoWindow = Instantiate(tradeInfoPanelPrefab, infoPanelParent);
        infoWindow.Animate(resourceAmounts);
        SkipDialog();
    }
}
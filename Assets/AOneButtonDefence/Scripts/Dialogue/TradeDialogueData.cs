using System.Collections.Generic;
using Adventurer;
using UnityEngine;

[CreateAssetMenu(fileName = "New TradeDialogueData", menuName = "Data/Trade Dialogue Data", order = 51)]
public class TradeDialogueData : DialogueData
{
    public List<StartResourceAmount> AcceptResources;
    public List<StartResourceAmount> DeclineResources;
}
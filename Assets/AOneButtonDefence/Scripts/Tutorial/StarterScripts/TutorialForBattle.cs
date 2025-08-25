using System;
using UnityEngine;

public class TutorialForBattle : TutorialObject
{
	[SerializeField] private int tutorialsForBattleAmount = 2;
	
	private Action handler;
	private int closedTutorialsAmount;
	
	private void Awake()
	{
		handler = delegate
		{
			isActivated = true;
			TimeManager.SetTimeScale(0.15f);
			TutorialMessage.TutorialWindowDestroyed += CheckBattleTutorialEnd;
			GameBattleState.BattleStarted -= handler;
		};

		GameBattleState.BattleStarted += handler;
	}

	public override void Dispose()
	{
		GameBattleState.BattleStarted -= handler;
	}

	private void CheckBattleTutorialEnd()
	{	
		if (++closedTutorialsAmount >= tutorialsForBattleAmount - 1)
		{
			Debug.Log(closedTutorialsAmount < tutorialsForBattleAmount);
			Debug.Log(closedTutorialsAmount + "" + tutorialsForBattleAmount);
			TutorialMessage.TutorialWindowDestroyed -= CheckBattleTutorialEnd;
			TutorialMessage.TutorialWindowDestroyed += ResetGameTime;
		}
	}

	private void ResetGameTime()
	{
		TimeManager.SetTimeScale(1f);
		TutorialMessage.TutorialWindowDestroyed -= ResetGameTime;
		Destroy(this);
	}
}
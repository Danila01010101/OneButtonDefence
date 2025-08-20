using System;
using UnityEngine;

public class TutorialForBattle : MonoBehaviour
{
	private Action handler;
	
	private void Awake()
	{
		handler = delegate
		{
			TutorialManager.TriggerTutorial();
			TimeManager.SetTimeScale(0.1f);
			TutorialMessage.TutorialWindowDestroyed += ResetGameTime;
			Destroy(this);
			GameBattleState.BattleStarted -= handler;
		};

		GameBattleState.BattleStarted += handler;
	}

	private void ResetGameTime()
	{
		TimeManager.SetTimeScale(1f);
		TutorialMessage.TutorialWindowDestroyed -= ResetGameTime;
	}
}
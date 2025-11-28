using System;
using UnityEngine;

namespace AOneButtonDefence.Scripts
{
    public class CharacterVisibilityToggler : IDisposable
    {
        private readonly PlayerController player;
        private readonly BattleEvents battleEvents;
        private Vector3 characterPositionForBattle;

        public CharacterVisibilityToggler(PlayerController player, Vector3 characterPositionForBattle, BattleEvents battleEvents)
        {
            this.player = player;
            this.characterPositionForBattle = characterPositionForBattle;
            this.battleEvents = battleEvents;

            HidePlayer();
            battleEvents.Subscribe(ShowPlayer, HidePlayer);
        }

        public void SetNewPosition(Vector3 newPosition) => characterPositionForBattle = newPosition;

        private void ShowPlayer() => player.Enable();

        private void HidePlayer()
        {
            player.Disable();
            player.transform.position = characterPositionForBattle;
        }

        public void Dispose()
        {
            battleEvents.Unsubscribe(ShowPlayer, HidePlayer);
        }
    }

    public class BattleEvents : IDisposable
    {
        private Action battleStartTrigger;
        private Action battleEndTrigger;

        private event Action OnBattleStart;
        private event Action OnBattleEnd;

        private bool disposed;

        public BattleEvents(Action battleStartAction, Action battleEndAction)
        {
            battleStartTrigger = battleStartAction;
            battleEndTrigger = battleEndAction;

            battleStartTrigger += TriggerStartHandlers;
            battleEndTrigger += TriggerEndHandlers;

            OnBattleStart += battleStartAction;
            OnBattleEnd += battleEndAction;
        }

        public void Subscribe(Action startHandler, Action endHandler)
        {
            OnBattleStart += startHandler;
            OnBattleEnd += endHandler;
        }

        public void Unsubscribe(Action startHandler, Action endHandler)
        {
            OnBattleStart -= startHandler;
            OnBattleEnd -= endHandler;
        }

        private void TriggerStartHandlers() => OnBattleStart?.Invoke();
        private void TriggerEndHandlers() => OnBattleEnd?.Invoke();

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            battleStartTrigger -= TriggerStartHandlers;
            battleEndTrigger -= TriggerEndHandlers;

            foreach (var d in OnBattleStart?.GetInvocationList() ?? Array.Empty<Delegate>())
                OnBattleStart -= (Action)d;

            foreach (var d in OnBattleEnd?.GetInvocationList() ?? Array.Empty<Delegate>())
                OnBattleEnd -= (Action)d;
        }
    }
}
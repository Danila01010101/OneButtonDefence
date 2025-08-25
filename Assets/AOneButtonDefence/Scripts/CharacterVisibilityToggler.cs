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

    public class BattleEvents
    {
        public void Subscribe(Action startHandler, Action endHandler)
        {
            GameBattleState.BattleStarted += startHandler;
            GameBattleState.BattleWon += endHandler;
        }

        public void Unsubscribe(Action startHandler, Action endHandler)
        {
            GameBattleState.BattleStarted -= startHandler;
            GameBattleState.BattleWon -= endHandler;
        }
    }
}
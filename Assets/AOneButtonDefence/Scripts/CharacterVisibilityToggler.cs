using System;
using UnityEngine;

namespace AOneButtonDefence.Scripts
{
    public class CharacterVisibilityToggler : IDisposable
    {
        private readonly PlayerController player;
        private readonly IBattleEvent battleEvents;
        private Vector3 characterPositionForBattle;

        public CharacterVisibilityToggler(PlayerController player, Vector3 characterPositionForBattle, IBattleEvent battleEvents)
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
}
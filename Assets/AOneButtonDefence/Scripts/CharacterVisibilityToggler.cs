using System;
using UnityEngine;

namespace AOneButtonDefence.Scripts
{
    public class CharacterVisibilityToggler : IDisposable
    {
        private readonly PlayerController player;

        private Vector3 characterPositionForBattle;
        private Action battleStartAction;
        private Action battleEndAction;

        public CharacterVisibilityToggler(PlayerController player, Vector3 characterPositionForBattle, Action battleStartAction, Action battleEndAction)
        {
            this.player = player;
            this.characterPositionForBattle = characterPositionForBattle;
            this.battleStartAction = battleStartAction;
            this.battleEndAction = battleEndAction;
            battleStartAction += OnBattleStart;
            battleEndAction += OnBattleEnd;
        }
        
        public void SetNewPosition(Vector3 newPosition) => characterPositionForBattle = newPosition;

        private void OnBattleStart()
        {
            player.transform.position = characterPositionForBattle;
            player.Disable();
        }

        private void OnBattleEnd()
        {
            player.Enable();
        }
        
        public void Dispose()
        {
            battleStartAction -= OnBattleStart;
            battleEndAction -= OnBattleEnd;
        }
    }
}
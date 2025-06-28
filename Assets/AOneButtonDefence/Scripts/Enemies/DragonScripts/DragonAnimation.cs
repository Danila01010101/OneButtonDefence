using System;
using AOneButtonDefence.Scripts.Interfaces;
using UnityEngine;

namespace AOneButtonDefence.Scripts.Enemies.DragonScripts
{
    public class DragonAnimation : MonoBehaviour, IAttackAnimator
    {
        public Action CharacterAttacked { get; set; }
        public Action CharacterAttackEnded { get; set; }
        public void InterruptAnimation()
        {
            CharacterAttackEnded?.Invoke();
        }

        public Transform Transform { get; set; }

        public void StartAnimation()
        {
            CharacterAttacked?.Invoke();
        }
    }
}
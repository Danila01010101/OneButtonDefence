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
            throw new NotImplementedException();
        }

        public Transform Transform { get; set; }

        public void StartAnimation()
        {
            throw new System.NotImplementedException();
        }

        public void InteruptAnimation()
        {
            throw new System.NotImplementedException();
        }
    }
}
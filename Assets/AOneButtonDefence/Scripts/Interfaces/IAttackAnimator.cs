using System;
using UnityEngine;

namespace AOneButtonDefence.Scripts.Interfaces
{
    public interface IAttackAnimator
    {
        Action CharacterAttacked { get; set; }
        Action CharacterAttackEnded { get; set; }
        void StartAnimation();
        void InterruptAnimation();
        Transform Transform { get; }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace AOneButtonDefence.Scripts.StateMachine
{
    public interface IUnitStateMachineWithEffects : IStateChanger
    {
        List<ActiveEffect> CurrentEffects { get; }
        Vector3 OriginalScale { get; set; }
        bool OriginalScaleInitialized { get; set; }

        void AddEffect(ActiveEffect effect);
        void RemoveEffect(ActiveEffect effect);
    }
    
    
    public class ActiveEffect
    {
        public Building.EffectCastInfo Info { get; }
        public IEffectActivator OriginActivator { get; }
        public ParticleSystem EffectInstance { get; set; }
        public float ScaleMultiplier { get; }

        public ActiveEffect(Building.EffectCastInfo info, IEffectActivator originActivator, float scaleMultiplier)
        {
            Info = info;
            OriginActivator = originActivator;
            ScaleMultiplier = scaleMultiplier;
        }
    }
}
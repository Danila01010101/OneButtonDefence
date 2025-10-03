using System.Collections.Generic;
using UnityEngine;

namespace AOneButtonDefence.Scripts.StateMachine
{
    public interface IUnitStateMachineWithEffects : IStateChanger
    {
        bool OriginalScaleInitialized { get; set; }
        Vector3 OriginalScale { get; set; }
        List<ActiveEffect> CurrentEffects { get; }

        void AddEffect(ActiveEffect effect);
        void RemoveEffect(ActiveEffect effect);

        void RecalculateScale();
    }
    
    public class ActiveEffect
    {
        public Building.EffectCastInfo Info { get; }
        public IEffectActivator OriginActivator { get; }
        public ParticleSystem EffectInstance { get; set; }
        public float ScaleMultiplier { get; }
        public Vector3 OriginalScale { get; set; }

        public ActiveEffect(Building.EffectCastInfo info, IEffectActivator originActivator, float scaleMultiplier, ParticleSystem instance)
        {
            Info = info;
            OriginActivator = originActivator;
            ScaleMultiplier = scaleMultiplier;
            EffectInstance = instance;

            if (EffectInstance != null)
            {
                OriginalScale = EffectInstance.transform.localScale;
                ApplyScale();
            }
        }

        public void ApplyScale()
        {
            if (EffectInstance != null)
                EffectInstance.transform.localScale = OriginalScale * ScaleMultiplier;
        }

        public void Enable()
        {
            if (EffectInstance != null)
            {
                ApplyScale();
                EffectInstance.Play();
            }
        }

        public void Disable()
        {
            if (EffectInstance != null)
            {
                EffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                EffectInstance.transform.localScale = Vector3.zero;
            }
        }

        public void Destroy()
        {
            if (EffectInstance != null)
            {
                GameObject.Destroy(EffectInstance.gameObject);
            }
        }
    }
}
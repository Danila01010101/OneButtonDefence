using System.Collections.Generic;
using UnityEngine;

namespace AOneButtonDefence.Scripts.StateMachine
{
    public interface IUnitStateMachineWithEffects : IStateChanger, IEffectsHandler
    {
        List<ActiveEffect> CurrentEffects { get; }
    }
    
    public class ActiveEffect
    {
        public Building.EffectCastInfo Info { get; }
        public IEffectActivator OriginActivator { get; }
        public ParticleSystem EffectInstance { get; set; }
        public CharacterStatsCounter StatsToChange { get; }
        public Vector3 OriginalScale { get; set; }
        
        public readonly float ScaleMultiplier = 0.2f;

        public ActiveEffect(Building.EffectCastInfo info, IEffectActivator originActivator, ParticleSystem instance, CharacterStatsCounter statsToChange)
        {
            Info = info;
            OriginActivator = originActivator;
            EffectInstance = instance;
            StatsToChange = statsToChange;

            if (EffectInstance != null)
            {
                OriginalScale = EffectInstance.transform.localScale;
                ApplyScale();
            }
        }

        public void ApplyScale()
        {
            if (EffectInstance != null)
                EffectInstance.transform.localScale = OriginalScale * ScaleMultiplier * Info.BuffResource.Amount;
        }

        public void Enable()
        {
            if (EffectInstance != null)
            {
                StatsToChange.ChangeStat(Info.BuffResource.Resource.Type, Info.BuffResource.Amount);
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
                StatsToChange.ChangeStat(Info.BuffResource.Resource.Type, -Info.BuffResource.Amount);
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
using AOneButtonDefence.Scripts.StateMachine;

public interface IEffectsHandler
{
    void AddEffect(ActiveEffect effect);
    void RemoveEffect(ActiveEffect effect);
    void RemoveEffectByActivator(IEffectActivator activator);
}
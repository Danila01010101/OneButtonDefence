using AOneButtonDefence.Scripts.StateMachine;

public interface IEffectReceiver
{
    void AddEffect(ActiveEffect effect);
    void RemoveEffect(ActiveEffect effect);
}
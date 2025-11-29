using System;

public interface IBattleEvent
{
    void Subscribe(Action startHandler, Action endHandler);
    void Unsubscribe(Action startHandler, Action endHandler);
}

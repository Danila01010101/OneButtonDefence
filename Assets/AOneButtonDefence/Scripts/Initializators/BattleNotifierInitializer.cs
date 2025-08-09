using System.Collections;

public class BattleNotifierInitializer : IGameInitializerStep
{
    public BattleNotifier Instance { get; private set; }

    public IEnumerator Initialize()
    {
        Instance = new BattleNotifier();
        yield break;
    }
}
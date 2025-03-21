public class BattleNotifier : IBattleNotifier
{
    private static BattleNotifier instance;

    private bool isBattleGoing;

    public static BattleNotifier Instance => instance;

    public bool IsBattleGoing() => isBattleGoing;

    public BattleNotifier()
    {
        instance = this;
    }

    public void Subscribe()
    {
        GameBattleState.BattleStarted += DetectBattleStart;
        GameBattleState.BattleWon += DetectBattleEnd;
        GameBattleState.BattleLost += DetectBattleEnd;
    }

    public void Unsubscribe()
    {
        GameBattleState.BattleStarted -= DetectBattleStart;
        GameBattleState.BattleWon -= DetectBattleEnd;
        GameBattleState.BattleLost -= DetectBattleEnd;
    }

    private void DetectBattleStart() => isBattleGoing = true;

    private void DetectBattleEnd() => isBattleGoing = false;
}

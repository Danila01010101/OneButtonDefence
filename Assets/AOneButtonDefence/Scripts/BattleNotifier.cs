public class BattleNotifier : IBattleNotifier
{
    private static BattleNotifier instance;

    private bool isBattleGoing;

    public static BattleNotifier Instance => instance;

    public bool IsBattleGoing() => isBattleGoing;

    public BattleNotifier()
    {
        GameBattleState.BattleStarted += DetectBattleStart;
        GameBattleState.BattleWon += DetectBattleEnd;
        GameBattleState.BattleLost += DetectBattleEnd;
        instance = this;
    }

    private void DetectBattleStart() => isBattleGoing = true;

    private void DetectBattleEnd() => isBattleGoing = false;
}

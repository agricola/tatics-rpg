public class EnemyTurnEvent : GameEvent
{
    public BattleGroup GoodGroup { get; private set; }
    public BattleGroup BadGroup { get; private set; }
    public EventStatus Status { get; private set; }
    public EnemyTurnEvent(BattleGroup good, BattleGroup bad, EventStatus status)
    {
        GoodGroup = good;
        BadGroup = bad;
        Status = status;
    }
}
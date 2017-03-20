using System.Collections.Generic;

public class SetBattleGroupsEvent : GameEvent
{
    public BattleGroup good;
    public BattleGroup bad;
    public SetBattleGroupsEvent(BattleGroup good, BattleGroup bad)
    {
        this.good = good;
        this.bad = bad;
    }
}
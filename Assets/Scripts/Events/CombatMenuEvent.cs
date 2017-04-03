public class CombatMenuEvent : GameEvent
{
    public bool fightOn;
    public bool waitOn;

    public CombatMenuEvent(bool fightOn = false, bool waitOn = false)
    {
        this.fightOn = fightOn;
        this.waitOn = waitOn;
    }
}

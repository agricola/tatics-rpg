public class CombatMenuEvent : GameEvent { }

public class ToggleCombatMenuEvent : CombatMenuEvent
{
    public bool menuOn;
    public ToggleCombatMenuEvent(bool menuOn)
    {
        this.menuOn = menuOn;
    }
}

public class ToggleCombatButtonsEvent : CombatMenuEvent
{
    public bool fightOn;
    public bool waitOn;
    public bool endOn;
    private bool v1;
    private bool v2;
    private bool v3;

    public ToggleCombatButtonsEvent(bool fightOn, bool waitOn, bool endOn)
    {
        this.fightOn = fightOn;
        this.waitOn = waitOn;
        this.endOn = endOn;
    }
}

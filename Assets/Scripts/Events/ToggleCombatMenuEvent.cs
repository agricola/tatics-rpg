
public class ToggleCombatMenuEvent : GameEvent
{
    public bool menuOn;
    public ToggleCombatMenuEvent(bool menuOn)
    {
        this.menuOn = menuOn;
    }
}

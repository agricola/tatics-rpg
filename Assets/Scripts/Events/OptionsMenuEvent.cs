public class OptionsMenuEvent : GameEvent
{
    public bool ForceOff { get; private set; }
    public OptionsMenuEvent(bool forceOff = false)
    {
        ForceOff = forceOff;
    }
}

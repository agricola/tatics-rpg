public class ColliderToggleEvent : GameEvent
{
    public bool Enabled { get; private set;}
    public ColliderToggleEvent(bool enabled)
    {
        Enabled = enabled;
    }
}
public class InputToggleEvent : GameEvent
{
    public bool inputEnabled;
    public InputToggleEvent(bool inputEnabled)
    {
        this.inputEnabled = inputEnabled;
    }
}

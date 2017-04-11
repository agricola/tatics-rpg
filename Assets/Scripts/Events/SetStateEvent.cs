public class SetStateEvent : GameEvent { }

public class SetInputStateEvent : SetStateEvent
{
    public IInputState state;
    public Character enterObj;
    public SetInputStateEvent(IInputState state, Character enterObj = null)
    {
        this.state = state;
        this.enterObj = enterObj;
    }
}
public class MoveCharacterEvent : GameEvent
{
    public Character Character { get; private set; }
    public bool Skip { get; private set; }
    public MoveCharacterEvent(Character character, bool skip)
    {
        Character = character;
        Skip = skip;
    }
}

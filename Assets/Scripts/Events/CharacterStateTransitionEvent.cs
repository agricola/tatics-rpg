public class CharacterStateTransitionEvent : GameEvent
{
    public ICharacterState CharacterState { get; private set; }
    public CharacterStateTransitionEvent(ICharacterState state)
    {
        CharacterState = state;
    }
}
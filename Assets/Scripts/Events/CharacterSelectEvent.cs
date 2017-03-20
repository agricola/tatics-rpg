public class CharacterSelectEvent : GameEvent
{
    public bool isGood;
    public Character character;
    public CharacterSelectEvent(bool isGood, Character character)
    {
        this.isGood = isGood;
        this.character = character;
    }
}
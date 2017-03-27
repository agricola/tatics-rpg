public class FightEvent : GameEvent
{
    public Character Attacker { get; private set; }
    public Character Defender { get; private set; }
    public FightEvent(Character attacker, Character defender)
    {
        Attacker = attacker;
        Defender = defender;
    }
}
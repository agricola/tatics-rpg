public class FinishCombatEvent : GameEvent
{
    public Character Attacker { get; private set; }
    public FinishCombatEvent(Character attacker)
    {
        Attacker = attacker;
    }
}
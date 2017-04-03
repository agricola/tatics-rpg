public class ChangeHealthEvent : GameEvent
{
    public int Damage { get; private set; }
    public Character Defender { get; private set; }
    public ChangeHealthEvent(int dmg, Character def)
    {
        Damage = dmg;
        Defender = def;
    }
}

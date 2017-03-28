public class FightResultEvent : GameEvent
{
    public AnimationManager AttackerAnimation { get; private set; }
    public AnimationManager DefenderAnimation { get; private set; }
    public Damageable DefenderDamageable { get; private set; }
    public int Damage { get; private set; }
    public FightResultEvent(
        AnimationManager atk,
        AnimationManager def,
        Damageable defDmgble,
        int dmg)
    {
        AttackerAnimation = atk;
        DefenderAnimation = def;
        DefenderDamageable = defDmgble;
        Damage = dmg;
    }
}

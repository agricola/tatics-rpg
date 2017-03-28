using UnityEngine;

public class TakeDamageEvent : GameEvent
{
    public GameObject Defender { get; private set; }
    public int Damage { get; private set; }
    public Vector2 Direction { get; private set; }
    public TakeDamageEvent(GameObject def, int dmg, Vector2 dir)
    {
        Defender = def;
        Damage = dmg;
        Direction = dir;
    }
}
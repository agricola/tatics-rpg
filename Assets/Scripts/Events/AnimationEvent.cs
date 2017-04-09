using UnityEngine;

public enum AnimationStatus { Start, Finish }

public class AnimationEvent : GameEvent
{
    public AnimationStatus Status { get; protected set; }
    public GameObject Actor { get; protected set; }
    public AnimationEvent(AnimationStatus status, GameObject actor)
    {
        Status = status;
        Actor = actor;
    }
}

public class AnimationWalkEvent : AnimationEvent

{
    public AnimationWalkEvent(AnimationStatus status, GameObject actor) : base(status, actor) { }
}

public class AnimationFightEvent : AnimationEvent
{
    public Character Attacker { get; private set; }
    public Character Defender { get; private set; }
    public AnimationFightEvent(
        AnimationStatus status,
        Character attacker,
        Character defender) : base(status, attacker.gameObject)
    {
        Attacker = attacker;
        Defender = defender;
    }
}

public class AnimationDeathEvent : AnimationEvent
{
    public AnimationDeathEvent(AnimationStatus status, GameObject actor) : base(status, actor) { }
}
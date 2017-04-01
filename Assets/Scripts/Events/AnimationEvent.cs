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
    public AnimationFightEvent(AnimationStatus status, GameObject actor) : base(status, actor) { }
}

public class AnimationDeathEvent : AnimationEvent
{
    public AnimationDeathEvent(AnimationStatus status, GameObject actor) : base(status, actor) { }
}
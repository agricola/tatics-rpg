using UnityEngine;

public class AnimationEvent : GameEvent
{
    public bool Act { get; protected set; }
    public GameObject Actor { get; protected set; }
    public AnimationEvent(bool act, GameObject actor)
    {
        Act = act;
        Actor = actor;
    }
}

public class ToggleWalkEvent : AnimationEvent
{
    public ToggleWalkEvent(bool act, GameObject actor) : base(act, actor) { }
}

public class ToggleFightEvent : AnimationEvent
{
    public ToggleFightEvent(bool act, GameObject actor) : base(act, actor) { }
}
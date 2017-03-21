using UnityEngine;

public class ToggleWalkEvent : GameEvent
{
    public bool walk;
    public GameObject walker;
    public ToggleWalkEvent(bool walk, GameObject walker)
    {
        this.walk = walk;
        this.walker = walker;
    }
}
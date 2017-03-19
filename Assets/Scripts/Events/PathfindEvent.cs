using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindEvent : GameEvent { }

public class PathfindCreateEvent : PathfindEvent
{
    public GameObject source;
    public GameObject goal;

    public PathfindCreateEvent(GameObject source, GameObject goal)
    {
        this.source = source;
        this.goal = goal;
    }
}

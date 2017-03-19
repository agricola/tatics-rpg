using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindEvent : GameEvent { }

public class CancelPathfindEvent : PathfindEvent { }

public class PathfindCreateEvent : PathfindEvent
{
    public GameObject source;
    public GameObject goal;
    public int limit;

    public PathfindCreateEvent(GameObject source, GameObject goal, int limit)
    {
        this.source = source;
        this.goal = goal;
        this.limit = limit;
    }
}

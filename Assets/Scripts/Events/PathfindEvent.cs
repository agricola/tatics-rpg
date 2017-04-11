public class PathfindEvent : GameEvent { }

public class CancelPathfindEvent : PathfindEvent { }

public class PathfindCreateEvent : PathfindEvent
{
    public Tile source;
    public Tile goal;
    public int limit;

    public PathfindCreateEvent(Tile source, Tile goal, int limit)
    {
        this.source = source;
        this.goal = goal;
        this.limit = limit;
    }
}

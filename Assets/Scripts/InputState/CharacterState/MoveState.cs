using System;
using UnityEngine;

public class MoveState : TargetState
{
    protected override void CreateTargetableTiles()
    {
        MapPosition center = selected.MapPosition;
        int radius = selected.MovementLimit;
        targetableTiles = RadiusManager.Instance.GetRadius(center, radius);
        requiresPathfindClear = true;
    }

    protected override void TileAction(Tile tile)
    {
        IssueMoveCommand(tile);
    }

    protected override void TileHighlight(Tile tile)
    {
        IssuePathfindCommand(tile);
    }

    private void IssueMoveCommand(Tile tile)
    {
        bool skip = tile.Occupant == selected.gameObject;
        EventManager.Instance.Raise(new MoveCharacterEvent(selected, skip));
    }

    private void IssuePathfindCommand(Tile goal)
    {
        //if (!selected || !inputEnabled) return;
        Map map = GameManager.Instance.CurrentMap;
        Tile source = map.TileAtMapPosition(selected.MapPosition);
        int limit = selected.MovementLimit;
        PathfindCreateEvent e = new PathfindCreateEvent(source, goal, limit);
        EventManager.Instance.Raise<PathfindEvent>(e);
    }
}

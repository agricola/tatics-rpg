using System;
using System.Collections.Generic;
using UnityEngine;

public class LineTargetState : TargetState
{
    private readonly Action<Tile> targetAction;

    public LineTargetState(Action<Tile> targetAction = null)
    {
        this.targetAction = targetAction;
    }
    protected override void CreateTargetableTiles()
    {
        if (selected)
        {
            targetableTiles = LineTiles(selected.MapPosition, selected.MovementLimit);
        }
        
    }

    protected override void TileAction(Tile tile)
    {
        if (targetAction != null) targetAction(tile);
    }

    protected override void TileHighlight(Tile tile)
    {
        List<Tile> tiles = StraightLine(selected.MapPosition, tile.MapPosition);
        EventManager.Instance.Raise(new HighlightEvent(HighlightSelection.Sub, tiles));
    }

    private List<Tile> LineTiles(MapPosition pos, int limit)
    {
        Map m = GameManager.Instance.CurrentMap;
        if (!m) return null;
        List<Tile> neighbors = m.GetTilesInCross(pos, limit);
        return neighbors;
    }

    private List<Tile> StraightLine(MapPosition pos, MapPosition goal)
    {
        Map m = GameManager.Instance.CurrentMap;
        if (!m) return null;
        List<Tile> neighbors = m.StraightPath(pos, goal);
        return neighbors;
    }
}
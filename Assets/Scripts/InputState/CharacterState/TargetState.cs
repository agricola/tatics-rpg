using System;
using System.Collections.Generic;
using System.Linq;

public abstract class TargetState : ICharacterState
{
    protected Character selected;
    protected List<Tile> targetableTiles;
    protected bool requiresPathfindClear = false;

    protected abstract void TileHighlight(Tile tile);
    protected abstract void TileAction(Tile tile);
    protected abstract void CreateTargetableTiles();

    public void Enter(Character selected = null, Map map = null)
    {
        this.selected = selected;
        EventManager.Instance.Raise(new CombatMenuEvent());
        CreateTargetableTiles();
        HighlightTiles(HighlightType.Radius);
    }

    public void Exit()
    {
        ClearTiles();
        if (requiresPathfindClear)
        {
            EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
        }
        // might need CancelpathfindEvent :DD
    }

    private void HighlightTiles(HighlightType highlightType)
    {
        if (targetableTiles == null || targetableTiles.Count <= 0) return;
        foreach (Tile tile in targetableTiles)
        {
            tile.Highlight(highlightType);
        }
    }

    private void ClearTiles()
    {
        HighlightTiles(HighlightType.None);
        targetableTiles = null; // is this even needed ????
    }

    public void HandleInput()
    {
        return;
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        return;
    }

    public void OnTileSelect(TileSelectEvent e)
    {
        var match = targetableTiles.Where(x => x == e.tile);
        if (!match.Contains(e.tile)) return;
        if (e.selectType == TileSelectType.Highlight)
        {
            TileHighlight(e.tile);
        }
        else if (e.selectType == TileSelectType.Move)
        {
            TileAction(e.tile);
            ClearTiles();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TargetState : ICharacterState
{
    protected Character selected;
    protected List<Tile> targetableTiles;
    protected bool requiresPathfindClear = false;

    protected abstract void TileHighlight(Tile tile);
    protected abstract void TileAction(Tile tile);
    protected abstract void CreateTargetableTiles();

    protected virtual void HighlightOther()
    {
        HighlightTiles();
        return;
    }

    public void Enter(Character selected = null, Map map = null)
    {
        this.selected = selected;
        EventManager.Instance.Raise(new CombatMenuEvent());
        CreateTargetableTiles();
        HighlightTiles();
    }

    public void Exit()
    {
        ClearTiles();
        if (requiresPathfindClear)
        {
            EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
        }
    }

    private void HighlightTiles()
    {
        EventManager.Instance.Raise(new HighlightEvent(
            HighlightSelection.Main,
            targetableTiles));
    }

    private void ClearTiles()
    {
        EventManager.Instance.Raise(new HighlightEvent(HighlightSelection.Main));
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
        if (targetableTiles == null || targetableTiles.Count <= 0) return;
        var match = targetableTiles.Where(x => x == e.tile);
        if (!match.Contains(e.tile))
        {
            HighlightOther();
            return;
        }
        if (e.selectType == TileSelectType.Highlight)
        {
            if (e.tile.Occupant == selected)
            {
                HighlightTiles();
            }
            else
            {
                TileHighlight(e.tile);
            }
        }
        else if (e.selectType == TileSelectType.Move)
        {
            TileAction(e.tile);
            ClearTiles();
        }
    }
}

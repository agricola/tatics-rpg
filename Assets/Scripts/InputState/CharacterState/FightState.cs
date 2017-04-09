using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightState : TargetState
{

    protected override void TileHighlight(Tile tile)
    {
        HighlightNeighbors(targetableTiles, HighlightType.Radius, HighlightEvilTile);
        HighlightEvilTile(tile, HighlightType.Targeting);
    }
    protected override void HighlightOther()
    {
        HighlightNeighbors(targetableTiles, HighlightType.Radius, HighlightEvilTile);
    }
    protected override void TileAction(Tile tile)
    {
        if (!tile.Occupant) return;
        InitiateFight(selected, tile.Occupant.GetComponent<Character>());
    }

    protected override void CreateTargetableTiles()
    {
        Map m = GameManager.Instance.CurrentMap;
        List<Tile> neighbors = m.GetNeighborsIncBlocked(selected.MapPosition);
        List<Tile> enemyNeighbors = neighbors.Where(x => x.Occupant != null).ToList();
        targetableTiles = enemyNeighbors;
        HighlightNeighbors(enemyNeighbors, HighlightType.Radius, HighlightEvilTile);
    }
    private void HighlightEvilTile(Tile tile, HighlightType type)
    {
        GameObject occ = tile.Occupant;
        if (!occ) return;
        Character c = occ.GetComponent<Character>();
        if (!c) return;
        if (!c.IsGood)
        {
            tile.Highlight(type);
        }
    }

    private void InitiateFight(Character attacker, Character defender)
    {
        EventManager.Instance.Raise(new InputToggleEvent(false));
        attacker.Acted = true;
        // too many raises!
        EventManager.Instance.Raise(new FightEvent(attacker, defender));
        EventManager.Instance.Raise(new SetInputStateEvent(new NoSelectionState(), selected));
    }

    private void HighlightNeighbors(
        List<Tile> neighbors,
        HighlightType type,
        Action<Tile, HighlightType> action)
    {
        if (neighbors.Count <= 0) return;
        foreach (Tile tile in neighbors)
        {
            action(tile, type);
        }
    }

}

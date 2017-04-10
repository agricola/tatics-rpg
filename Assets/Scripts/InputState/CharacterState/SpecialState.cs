using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialState : TargetState
{
    private SelectType selectType;
    private TargetType targetType;
    private int limit = 0;
    protected override void CreateTargetableTiles()
    {
        GenerateTilesBasedOnSelectType(selectType, limit);
    }

    private void GenerateTilesBasedOnSelectType(SelectType selectType, int limit)
    {
        MapPosition pos = selected.MapPosition;
        ActionBasedOnSelectType(selectType,
            SetMeleeTiles,
            () => SetLineTiles(pos, limit),
            () => targetableTiles =  RadiusManager.Instance.GetRadius(pos, limit));
    }

    private void ActionBasedOnSelectType(SelectType selectType,
        Action melee = null,
        Action line = null,
        Action ranged = null,
        Action def = null,
        Action none = null)
    {
        switch (selectType)
        {
            case SelectType.None:
                if (none != null) none();
                break;
            case SelectType.Melee:
                if (melee != null) melee();
                break;
            case SelectType.Line:
                if (line != null) line();
                break;
            case SelectType.Ranged:
                if (ranged != null) ranged();
                break;
            default:
                if (def != null) def();
                break;
        }
    }

    private bool IsTileTargetable(Tile tile)
    {
        var matches = targetableTiles.Where(x => x == tile);
        return matches.Contains(tile);
    }

    private bool IsTargetValid(Tile tile)
    {
        bool valid = false;
        switch (targetType)
        {
            case TargetType.Ally:
                if (!tile.Occupant) return false;
                valid = tile.Occupant.GetComponent<Character>().IsGood;
                break;
            case TargetType.Enemy:
                if (!tile.Occupant) return false;
                valid = !tile.Occupant.GetComponent<Character>().IsGood;
                break;
            case TargetType.Tile:
                valid = !tile.Blocked;
                break;
            case TargetType.Character:
                valid = tile.Occupant != null;
                break;
            default:
                valid = false;
                break;
        }
        return valid;
    }

    private void SetLineTiles(MapPosition pos, int limit)
    {
        Map m = GameManager.Instance.CurrentMap;
        List<Tile> neighbors = m.GetTilesInCross(pos, limit);
        targetableTiles = neighbors;
    }

    private void SetMeleeTiles()
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

    protected override void TileAction(Tile tile)
    {
        throw new NotImplementedException();
    }

    protected override void TileHighlight(Tile tile)
    {
        throw new NotImplementedException();
    }
}
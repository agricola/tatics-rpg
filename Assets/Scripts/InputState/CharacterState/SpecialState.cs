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
        switch (selectType)
        {
            case SelectType.None:
                break;
            case SelectType.Melee:
                SetMeleeTiles();
                break;
            case SelectType.Line:
                GenerateLineTiles(selected.MapPosition, limit);
                break;
            case SelectType.Ranged:
                targetableTiles = 
                    RadiusManager.Instance.GetRadius(selected.MapPosition, limit);
                break;
            default:
                break;
        }
    }

    private void GenerateLineTiles(MapPosition pos, int limit)
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
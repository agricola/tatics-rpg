using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightState : TargetState
{

    protected override void TileHighlight(Tile tile)
    {
        HighlightNeighbors(targetableTiles, true);
        if (IsEvilTile(tile)) HighlightTarget(tile);
    }
    protected override void HighlightOther()
    {
        HighlightNeighbors(targetableTiles, true);
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
    }
    private bool IsEvilTile(Tile tile)
    {
        GameObject occ = tile.Occupant;
        if (!occ) return false;
        Character c = occ.GetComponent<Character>();
        if (!c) return false;
        return !c.IsGood;
    }

    private void InitiateFight(Character attacker, Character defender)
    {
        EventManager.Instance.Raise(new InputToggleEvent(false));
        attacker.Acted = true;
        // too many raises!
        EventManager.Instance.Raise(new FightEvent(attacker, defender));
        EventManager.Instance.Raise(new SetInputStateEvent(new NoSelectionState(), selected));
    }

    private void HighlightNeighbors(List<Tile> neighbors, bool isOn)
    {
        UpdateTilesEvent e = new UpdateTilesEvent(HighlightSelection.Main, neighbors, isOn);
        EventManager.Instance.Raise<HighlightEvent>(e);
    }

    private void HighlightTarget(Tile target)
    {
        List<Tile> targets = new List<Tile>() { target };
        UpdateTilesEvent e = new UpdateTilesEvent(HighlightSelection.Sub, targets, true);
        EventManager.Instance.Raise<HighlightEvent>(e);
    }

    private void UnHighlightTarget()
    {
        EventManager.Instance.Raise<HighlightEvent>(new HighlightEvent(HighlightSelection.Sub));
    }

}

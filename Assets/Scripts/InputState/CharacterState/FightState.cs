using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightState : ICharacterState
{
    private Character selected;
    private List<Tile> neighbors;

    public void Enter(Character selected = null, Map map = null)
    {
        this.selected = selected;
        Map m = GameManager.Instance.Map;
        neighbors = m.GetNeighborsIncBlocked(selected.MapPosition);
        HighlightNeighbors(neighbors, HighlightType.Radius, HighlightEvilTile);
        EventManager.Instance.Raise(new CombatMenuEvent());
        
        //InitiateFight(selected, selected);
        // EventManager.Instance.Raise(new CombatMenuEvent(false, true, true));
    }

    public void Exit()
    {
        // send out end fight event
        HighlightNeighbors(neighbors, HighlightType.None, HighlightTile);
       
        return;
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
        if (selected.Acted) return;
        var match = neighbors.Where(x => x == e.tile);
        if (!match.Contains(e.tile)) return;
        if (e.selectType == TileSelectType.Highlight)
        {
            HighlightNeighbors(neighbors, HighlightType.Radius, HighlightEvilTile);
            HighlightEvilTile(e.tile, HighlightType.Targeting);
        }
        else if (e.selectType == TileSelectType.Move)
        {
            if (!e.tile.Occupant) return;
            InitiateFight(selected, e.tile.Occupant.GetComponent<Character>());
        }
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

    private void HighlightTile(Tile tile, HighlightType type)
    {
        tile.Highlight(type);
    }

    private void InitiateFight(Character attacker, Character defender)
    {
        EventManager.Instance.Raise(new InputToggleEvent(false));
        attacker.Acted = true;
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
